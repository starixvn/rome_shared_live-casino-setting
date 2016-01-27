namespace Fanex.LiveCasino.CoreSetting.Web.Filters
{
    using System;
    using System.Collections.Generic;
    using System.Web;
    using System.Web.Helpers;
    using System.Web.Mvc;
    using Fanex.Common;
    using Fanex.LiveCasino.CoreSetting.Configuration;
    using Fanex.LiveCasino.CoreSetting.Enumerations;
    using Fanex.LiveCasino.CoreSetting.Models;
    using Fanex.LiveCasino.CoreSetting.Resources;
    using Fanex.LiveCasino.CoreSetting.Services;
    using Fanex.LiveCasino.CoreSetting.Web.Binders;
    using Sunplus.Agent.Utilities;    
    
    /// <summary>
    /// Live casino authorization filter.
    /// </summary>
    public class LiveCasinoAuthorizationAttribute : AuthorizeAttribute
    {
        private const string AntiForgeryFormTokenName = "__RequestVerificationToken";

        public bool IsHost { get; set; }

        public ILiveCasinoAuthorizationService LiveCasinoAuthenticationService { get; set; }

        /// <summary>
        /// Execute before each action for authorization.
        /// </summary>
        /// <param name="filterContext">Current action context.</param>
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);

            if (!string.IsNullOrEmpty(LiveCasinoSettings.Settings.AuthorizationServiceType))
            {
                var authorizationType = Type.GetType(LiveCasinoSettings.Settings.AuthorizationServiceType);
                LiveCasinoAuthenticationService = Activator.CreateInstance(authorizationType) as ILiveCasinoAuthorizationService;

                var isAuthorized = false;
                if (filterContext.ActionDescriptor.ActionName.ToLower().Contains("multiple"))
                {
                    isAuthorized = IsHost ? IsAuthorizedHostMultiple(filterContext) : IsAuthorizedReadMultiple(filterContext);
                }
                else
                {
                    isAuthorized = IsHost ? IsAuthorizedHost(filterContext) : IsAuthorizedRead(filterContext);
                }

                if (IsHost && isAuthorized)
                {
                    isAuthorized = ValidateAntiXSRF();
                }

                if (!isAuthorized)
                {
                    UnAuthorizedAction(filterContext);
                }
            }
        }

        private bool ValidateAntiXSRF()
        {
            try
            {
                var request = HttpContext.Current.Request;

                var formToken = request.Headers[AntiForgeryFormTokenName];
                if (string.IsNullOrWhiteSpace(formToken))
                {
                    return false;
                }

                var cookieValue = request.Cookies[AntiForgeryConfig.CookieName];
                if (cookieValue != null
                    && !string.IsNullOrWhiteSpace(cookieValue.Value))
                {
                    AntiForgery.Validate(cookieValue.Value, formToken);
                }
                else
                {
                    Logs.WriteWarn(new Exception("Cannot read Request Verification Token from cookie."));
                }
            }
            catch (Exception ex)
            {
                Logs.WriteWarn(ex);

                return false;
            }

            return true;
        }

        private string GetActionParameterValue(IValueProvider valueProvider, string parameterName)
        {
            return valueProvider.GetValue(parameterName) != null
                                ? valueProvider.GetValue(parameterName).AttemptedValue
                                : string.Empty;
        }

        /// <summary>
        /// Get Setting context.
        /// </summary>
        /// <param name="valueProvider">Provider for getting data.</param>
        /// <returns>Setting context.</returns>
        private LiveCasinoSettingContextMultiple GetAjaxPostInformation(IValueProvider valueProvider)
        {
            var settingContext = new LiveCasinoSettingContextMultiple
            {
                Customer = new Customer
                {
                    Id = Converter.ToInt(GetActionParameterValue(valueProvider, "settingContext.Customer.Id")),
                    Level = (CustomerLevel)Converter.ToInt(GetActionParameterValue(valueProvider, "settingContext.Customer.Level")),
                    Username = GetActionParameterValue(valueProvider, "settingContext.Customer.Username")
                },
                Upline = new Customer
                {
                    Id = Converter.ToInt(GetActionParameterValue(valueProvider, "settingContext.Upline.Id")),
                    Level = (CustomerLevel)Converter.ToInt(GetActionParameterValue(valueProvider, "settingContext.Upline.Level"))
                },
                SubAccount = new User
                {
                    Id = Converter.ToInt(GetActionParameterValue(valueProvider, "settingContext.SubAccount.Id"))
                },
                Actor = new User
                {
                    Id = Converter.ToInt(GetActionParameterValue(valueProvider, "settingContext.Actor.Id")),
                    Username = Converter.To<string>(GetActionParameterValue(valueProvider, "settingContext.Actor.Username"))
                }
            };
            return settingContext;
        }

        /// <summary>
        /// Authorize user have permission host.
        /// </summary>
        /// <param name="filterContext">Current filter for checking authorization.</param>
        /// <returns>Is authorized or not.</returns>
        private bool IsAuthorizedHost(AuthorizationContext filterContext)
        {
            var valueProvider = filterContext.Controller.ValueProvider;
            var settingContext = GetAjaxPostInformation(valueProvider);

            return LiveCasinoAuthenticationService.IsAuthorizedHost(filterContext, settingContext);
        }

        /// <summary>
        /// Authorize user have permission host on multiple.
        /// </summary>
        /// <param name="filterContext">Current filter for checking authorization.</param>
        /// <returns>Is authorized or not.</returns>
        private bool IsAuthorizedHostMultiple(AuthorizationContext filterContext)
        {
            var valueProvider = filterContext.Controller.ValueProvider;
            LiveCasinoSettingContextMultiple settingContext = GetAjaxPostInformation(valueProvider);
            settingContext.Customers = new List<Customer>();
            int customerIndex = 0;
            string keyFormat = "settingContext.Customers[{0}]";
            string customerIdKeyFormat = "settingContext.Customers[{0}].Id";
            string usernameKeyFormat = "settingContext.Customers[{0}].Username";
            string syncKeyFormat = "settingContext.Customers[{0}].IsSyncLiveCasino";
            while (valueProvider.ContainsPrefix(string.Format(keyFormat, customerIndex)))
            {
                settingContext.Customers.Add(new Customer
                {
                    Id = Converter.ToInt(GetActionParameterValue(valueProvider, string.Format(customerIdKeyFormat, customerIndex))),
                    Username = GetActionParameterValue(valueProvider, string.Format(usernameKeyFormat, customerIndex)),
                    IsSyncLiveCasino = Converter.ToBool(GetActionParameterValue(valueProvider, string.Format(syncKeyFormat, customerIndex))),
                });

                customerIndex++;
            }

            return LiveCasinoAuthenticationService.IsAuthorizedHostMultiple(filterContext, settingContext);
        }

        /// <summary>
        /// Authorize user have permission read.
        /// </summary>
        /// <param name="filterContext">Current filter for checking authorization.</param>
        /// <returns>Is authorized or not.</returns>
        private bool IsAuthorizedRead(AuthorizationContext filterContext)
        {
            var settingContext = LiveCasinoSettingContextModelBinder.GetLiveCasinoSettingContext(filterContext.HttpContext.Request);

            return LiveCasinoAuthenticationService.IsAuthorizedRead(filterContext, settingContext);
        }

        /// <summary>
        /// Authorize user have permission read multiple.
        /// </summary>
        /// <param name="filterContext">Current filter for checking authorization.</param>
        /// <returns>Is authorized or not.</returns>
        private bool IsAuthorizedReadMultiple(AuthorizationContext filterContext)
        {
            var settingContext = LiveCasinoSettingContextMultipleModelBinder
                                .GetLiveCasinoSettingContextMultiple(filterContext.HttpContext.Request.Form);

            return LiveCasinoAuthenticationService.IsAuthorizedReadMultiple(filterContext, settingContext);
        }

        /// <summary>
        /// Un authorized action.
        /// </summary>
        /// <param name="filterContext">Filter context for un authorized action.</param>
        private void UnAuthorizedAction(AuthorizationContext filterContext)
        {
            HttpResponseBase response = filterContext.HttpContext.Response;

            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                response.Clear();
                response.Write(LiveCasinoLang.UnauthorizeMsg);
                filterContext.Result = new HttpUnauthorizedResult();
                filterContext.Result.ExecuteResult(filterContext.Controller.ControllerContext);
                response.End();
            }
            else
            {
                filterContext.Result = new RedirectResult("~/LiveCasinoSetting/LiveCasinoUser/UnAuthorized");
            }
        }
    }
}