namespace Fanex.LiveCasino.CoreSetting.Web.Filters
{
    using System;
    using System.Collections.Specialized;
    using System.Web;
    using System.Web.Mvc;
    using Fanex.Common;
    using Fanex.LiveCasino.CoreSetting.Configuration;
    using Fanex.LiveCasino.CoreSetting.Enumerations;
    using Fanex.LiveCasino.CoreSetting.Models;
    using Fanex.LiveCasino.CoreSetting.Resources;
    using Fanex.LiveCasino.CoreSetting.Services;
    using Fanex.LiveCasino.CoreSetting.Web.Binders;

    /// <summary>
    /// Live casino authorization filter.
    /// </summary>
    public class LiveCasinoAuthorizationAttribute : AuthorizeAttribute
    {
        public ILiveCasinoAuthorizationService LiveCasinoAuthenticationService { get; set; }

        public bool IsHost { get; set; }

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

                if (!isAuthorized)
                {
                    UnAuthorizedAction(filterContext);
                }
            }
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            return true;
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

        private bool IsAuthorizedReadMultiple(AuthorizationContext filterContext)
        {
            var settingContext = LiveCasinoSettingContextMultipleModelBinder
                                .GetLiveCasinoSettingContextMultiple(filterContext.HttpContext.Request.Form);

            return LiveCasinoAuthenticationService.IsAuthorizedReadMultiple(filterContext, settingContext);
        }

        /// <summary>
        /// Authorize user have permission host.
        /// </summary>
        /// <param name="filterContext">Current filter for checking authorization.</param>
        /// <returns>Is authorized or not.</returns>
        private bool IsAuthorizedHost(AuthorizationContext filterContext)
        {
            var valueProvider = filterContext.Controller.ValueProvider;
            var settingContext = new LiveCasinoSettingContext
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
                ActorId = Converter.ToInt(GetActionParameterValue(valueProvider, "settingContext.ActorId"))
            };

            return LiveCasinoAuthenticationService.IsAuthorizedHost(filterContext, settingContext);
        }

        private bool IsAuthorizedHostMultiple(AuthorizationContext filterContext)
        {
            var valueProvider = filterContext.Controller.ValueProvider;
            var settingContextMultiple = new LiveCasinoSettingContextMultiple
            {
                //ParentId = Converter.ToInt(GetActionParameterValue(valueProvider, "settingContext.ParentId")),
                //ParentLevel = (CustomerLevel)Converter.ToInt(GetActionParameterValue(valueProvider, "settingContext.ParentLevel")),
                //SubAccId = Converter.ToInt(GetActionParameterValue(valueProvider, "settingContext.SubAccId")),
                //Admin = Converter.ToInt(GetActionParameterValue(valueProvider, "settingContext.AdminId")),
                //CustomerId = Converter.ToInt(GetActionParameterValue(valueProvider, "settingContext.CustomerId")),
                //Username = GetActionParameterValue(valueProvider, "settingContext.Username"),
                //CustomerLevel = (CustomerLevel)Converter.ToInt(GetActionParameterValue(valueProvider, "settingContext.CustomerLevel")),
                //CustomerIds = GetActionParameterValue(valueProvider, "settingContext.CustomerIds"),
                //Usernames = GetActionParameterValue(valueProvider, "settingContext.Usernames"),
                //CustomerSyncedStatus = GetActionParameterValue(valueProvider, "settingContext.CustomerSyncedStatus")
            };

            return LiveCasinoAuthenticationService.IsAuthorizedHostMultiple(filterContext, settingContextMultiple);
        }

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
                filterContext.Result = new RedirectResult("~/LiveCasinoSetting/User/UnAuthorized");
            }
        }

        private string GetActionParameterValue(IValueProvider valueProvider, string parameterName)
        {
            
            return valueProvider.GetValue(parameterName) != null
                                ? valueProvider.GetValue(parameterName).AttemptedValue
                                : string.Empty;
        }
    }
}