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
            var valueProvider = filterContext.Controller.ValueProvider;
            var formCollection = filterContext.HttpContext.Request.Form;
            var settingContext = new SettingContext
            {
                ParentId = Converter.ToInt(GetFormValue(formCollection, "uplineId")),
                ParentLevel = (CustomerLevel)Converter.ToInt(GetFormValue(formCollection, "uplineRoleId")),
                SubAccId = Converter.ToInt(GetFormValue(formCollection, "subAccId")),
                AdminId = Converter.ToInt(GetFormValue(formCollection, "adminId")),
                CustomerId = Converter.ToInt(GetFormValue(formCollection, "targetCustId")),
                CustomerLevel = (CustomerLevel)Converter.ToInt(GetFormValue(formCollection, "targetRoleId"))
            };

            return LiveCasinoAuthenticationService.IsAuthorizedRead(filterContext, settingContext);
        }

        private bool IsAuthorizedReadMultiple(AuthorizationContext filterContext)
        {
            var valueProvider = filterContext.Controller.ValueProvider;
            var formCollection = filterContext.HttpContext.Request.Form;
            var settingContext = new SettingContextMultiple
            {
                ParentId = Converter.ToInt(GetFormValue(formCollection, "uplineId")),
                ParentLevel = (CustomerLevel)Converter.ToInt(GetFormValue(formCollection, "uplineRoleId")),
                SubAccId = Converter.ToInt(GetFormValue(formCollection, "subAccId")),
                AdminId = Converter.ToInt(GetFormValue(formCollection, "adminId")),
                CustomerId = Converter.ToInt(GetFormValue(formCollection, "targetCustId")),
                CustomerLevel = (CustomerLevel)Converter.ToInt(GetFormValue(formCollection, "targetRoleId")),
                Username = GetFormValue(formCollection, "targetUsername"),
                TargetCustIds = GetFormValue(formCollection, "targetCustIds"),
                TargetUsernames = GetFormValue(formCollection, "targetUsernames"),
            };

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
            var settingContext = new SettingContext
            {
                ParentId = Converter.ToInt(GetActionParameterValue(valueProvider, "settingContext.UplineId")),
                ParentLevel = (CustomerLevel)Converter.ToInt(GetActionParameterValue(valueProvider, "settingContext.UplineRoleId")),
                SubAccId = Converter.ToInt(GetActionParameterValue(valueProvider, "settingContext.SubAccId")),
                AdminId = Converter.ToInt(GetActionParameterValue(valueProvider, "settingContext.AdminId")),
                CustomerId = Converter.ToInt(GetActionParameterValue(valueProvider, "settingContext.TargetCustId")),
                Username = GetActionParameterValue(valueProvider, "settingContext.TargetUsername"),
                CustomerLevel = (CustomerLevel)Converter.ToInt(GetActionParameterValue(valueProvider, "settingContext.TargetRoleId"))
            };

            return LiveCasinoAuthenticationService.IsAuthorizedHost(filterContext, settingContext);
        }

        private bool IsAuthorizedHostMultiple(AuthorizationContext filterContext)
        {
            var valueProvider = filterContext.Controller.ValueProvider;
            var settingContextMultiple = new SettingContextMultiple
            {
                ParentId = Converter.ToInt(GetActionParameterValue(valueProvider, "settingContext.UplineId")),
                ParentLevel = (CustomerLevel)Converter.ToInt(GetActionParameterValue(valueProvider, "settingContext.UplineRoleId")),
                SubAccId = Converter.ToInt(GetActionParameterValue(valueProvider, "settingContext.SubAccId")),
                Admin = Converter.ToInt(GetActionParameterValue(valueProvider, "settingContext.AdminId")),
                CustomerId = Converter.ToInt(GetActionParameterValue(valueProvider, "settingContext.TargetCustId")),
                Username = GetActionParameterValue(valueProvider, "settingContext.TargetUsername"),
                CustomerLevel = (CustomerLevel)Converter.ToInt(GetActionParameterValue(valueProvider, "settingContext.TargetRoleId")),
                CustomerIds = GetActionParameterValue(valueProvider, "settingContext.TargetCustIds"),
                Usernames = GetActionParameterValue(valueProvider, "settingContext.TargetUsernames"),
                CustomerSyncedStatus = GetActionParameterValue(valueProvider, "settingContext.TargetSyncStatus")
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

        private string GetFormValue(NameValueCollection formCollection, string formKey)
        {
            return formCollection[formKey] ?? string.Empty;
        }
    }
}