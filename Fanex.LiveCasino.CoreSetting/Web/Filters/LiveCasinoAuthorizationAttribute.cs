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

                var isAuthorized = IsHost ? IsAuthorizedHost(filterContext) : IsAuthorizedRead(filterContext);

                if (!isAuthorized)
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
            var settingContext = new SettingContextMultiple();
            if (formCollection.Count > 0)
            {
                settingContext.ParentId = Converter.ToInt(GetFormValue(formCollection, "uplineId"));
                settingContext.ParentLevel = (CustomerLevel)Converter.ToInt(GetFormValue(formCollection, "uplineRoleId"));
                settingContext.SubAccId = Converter.ToInt(GetFormValue(formCollection, "subAccId"));
                settingContext.AdminId = Converter.ToInt(GetFormValue(formCollection, "adminId"));
                settingContext.CustomerId = Converter.ToInt(GetFormValue(formCollection, "targetCustId"));
                settingContext.CustomerLevel = (CustomerLevel)Converter.ToInt(GetFormValue(formCollection, "targetRoleId"));
                settingContext.Username = GetFormValue(formCollection, "targetUsername");
                settingContext.CustomerIds = GetFormValue(formCollection, "targetCustIds");
                settingContext.Usernames = GetFormValue(formCollection, "targetUsernames");
            }
            else
            {
                settingContext.ParentId = Converter.ToInt(GetActionParameterValue(valueProvider, "uplineId"));
                settingContext.ParentLevel = (CustomerLevel)Converter.ToInt(GetActionParameterValue(valueProvider, "uplineRoleId"));
                settingContext.SubAccId = Converter.ToInt(GetActionParameterValue(valueProvider, "subAccId"));
                settingContext.AdminId = Converter.ToInt(GetActionParameterValue(valueProvider, "adminId"));
                settingContext.CustomerId = Converter.ToInt(GetActionParameterValue(valueProvider, "targetCustId"));
                settingContext.CustomerLevel = (CustomerLevel)Converter.ToInt(GetActionParameterValue(valueProvider, "targetRoleId"));
                settingContext.Username = GetActionParameterValue(valueProvider, "targetUsername");
                settingContext.CustomerIds = GetActionParameterValue(valueProvider, "targetCustIds");
                settingContext.Usernames = GetActionParameterValue(valueProvider, "targetUsernames");
            }

            return LiveCasinoAuthenticationService.IsAuthorizedRead(filterContext, settingContext);
        }

        /// <summary>
        /// Authorize user have permission host.
        /// </summary>
        /// <param name="filterContext">Current filter for checking authorization.</param>
        /// <returns>Is authorized or not.</returns>
        private bool IsAuthorizedHost(AuthorizationContext filterContext)
        {
            var valueProvider = filterContext.Controller.ValueProvider;
            var settingContext = new SettingContextMultiple
            {
                ParentId = Converter.ToInt(GetActionParameterValue(valueProvider, "settingContext.UplineId")),
                ParentLevel = (CustomerLevel)Converter.ToInt(GetActionParameterValue(valueProvider, "settingContext.UplineRoleId")),
                SubAccId = Converter.ToInt(GetActionParameterValue(valueProvider, "settingContext.SubAccId")),
                AdminId = Converter.ToInt(GetActionParameterValue(valueProvider, "settingContext.AdminId")),
                CustomerId = Converter.ToInt(GetActionParameterValue(valueProvider, "settingContext.TargetCustId")),
                Username = GetActionParameterValue(valueProvider, "settingContext.TargetUsername"),
                CustomerLevel = (CustomerLevel)Converter.ToInt(GetActionParameterValue(valueProvider, "settingContext.TargetRoleId")),
                CustomerIds = GetActionParameterValue(valueProvider, "settingContext.TargetCustIds"),
                Usernames = GetActionParameterValue(valueProvider, "settingContext.TargetUsernames"),
                CustomerSyncedStatus = GetActionParameterValue(valueProvider, "settingContext.TargetSyncStatus")
            };
            return LiveCasinoAuthenticationService.IsAuthorizedHost(filterContext, settingContext);
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