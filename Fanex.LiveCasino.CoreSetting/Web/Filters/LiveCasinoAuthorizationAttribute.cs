namespace Fanex.LiveCasino.CoreSetting.Web.Filters
{
    using System;
    using System.Collections.Specialized;
    using System.Web;
    using System.Web.Mvc;
    using Fanex.Common;
    using Fanex.LiveCasino.CoreSetting.Models;
    using Fanex.LiveCasino.CoreSetting.Resources;
    using Fanex.LiveCasino.CoreSetting.Services;
    using Fanex.LiveCasino.CoreSetting.Utilities;

    /// <summary>
    /// Live casino authorization filter.
    /// </summary>
    public class LiveCasinoAuthorizationAttribute : AuthorizeAttribute
    {
        private readonly ICommonUtil _commonUtil = new CommonUtil();

        public ILiveCasinoAuthorizationService LiveCasinoAuthenticationService { get; set; }

        public bool IsHost { get; set; }

        /// <summary>
        /// Execute before each action for authorization.
        /// </summary>
        /// <param name="filterContext">Current action context.</param>
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);

            if (!string.IsNullOrEmpty(_commonUtil.LiveCasinoSettings.AuthorizationServiceType))
            {
                var authorizationType = Type.GetType(_commonUtil.LiveCasinoSettings.AuthorizationServiceType);
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
                        filterContext.Result = new RedirectResult("~/LiveCasinoSetting/LiveCasinoUser/UnAuthorized");
                    }
                }
            }
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
            var settingContext = new SettingContext();
            if (formCollection.Count > 0)
            {
                settingContext.UplineId = Converter.ToInt(GetFormValue(formCollection, "uplineId"));
                settingContext.UplineRoleId = Converter.ToInt(GetFormValue(formCollection, "uplineRoleId"));
                settingContext.SubAccId = Converter.ToInt(GetFormValue(formCollection, "subAccId"));
                settingContext.AdminId = Converter.ToInt(GetFormValue(formCollection, "adminId"));
                settingContext.TargetCustId = Converter.ToInt(GetFormValue(formCollection, "targetCustId"));
                settingContext.TargetRoleId = Converter.ToInt(GetFormValue(formCollection, "targetRoleId"));
                settingContext.TargetUsername = GetFormValue(formCollection, "targetUsername");
                settingContext.TargetCustIds = GetFormValue(formCollection, "targetCustIds");
                settingContext.TargetUsernames = GetFormValue(formCollection, "targetUsernames");
            }
            else
            {
                settingContext.UplineId = Converter.ToInt(GetActionParameterValue(valueProvider, "uplineId"));
                settingContext.UplineRoleId = Converter.ToInt(GetActionParameterValue(valueProvider, "uplineRoleId"));
                settingContext.SubAccId = Converter.ToInt(GetActionParameterValue(valueProvider, "subAccId"));
                settingContext.AdminId = Converter.ToInt(GetActionParameterValue(valueProvider, "adminId"));
                settingContext.TargetCustId = Converter.ToInt(GetActionParameterValue(valueProvider, "targetCustId"));
                settingContext.TargetRoleId = Converter.ToInt(GetActionParameterValue(valueProvider, "targetRoleId"));
                settingContext.TargetUsername = GetActionParameterValue(valueProvider, "targetUsername");
                settingContext.TargetCustIds = GetActionParameterValue(valueProvider, "targetCustIds");
                settingContext.TargetUsernames = GetActionParameterValue(valueProvider, "targetUsernames");
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
            var settingContext = new SettingContext
            {
                UplineId = Converter.ToInt(GetActionParameterValue(valueProvider, "settingContext.UplineId")),
                UplineRoleId = Converter.ToInt(GetActionParameterValue(valueProvider, "settingContext.UplineRoleId")),
                SubAccId = Converter.ToInt(GetActionParameterValue(valueProvider, "settingContext.SubAccId")),
                AdminId = Converter.ToInt(GetActionParameterValue(valueProvider, "settingContext.AdminId")),
                TargetCustId = Converter.ToInt(GetActionParameterValue(valueProvider, "settingContext.TargetCustId")),
                TargetUsername = GetActionParameterValue(valueProvider, "settingContext.TargetUsername"),
                TargetRoleId = Converter.ToInt(GetActionParameterValue(valueProvider, "settingContext.TargetRoleId")),
                TargetCustIds = GetActionParameterValue(valueProvider, "settingContext.TargetCustIds"),
                TargetUsernames = GetActionParameterValue(valueProvider, "settingContext.TargetUsernames"),
                TargetSyncStatus = GetActionParameterValue(valueProvider, "settingContext.TargetSyncStatus")
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