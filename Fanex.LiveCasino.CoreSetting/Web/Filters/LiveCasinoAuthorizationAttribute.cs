namespace Fanex.LiveCasino.CoreSetting.Web.Filters
{
    using System.Web;
    using System.Web.Mvc;
    using Fanex.Common;
    using Fanex.LiveCasino.CoreSetting.Models;
    using Fanex.LiveCasino.CoreSetting.Resources;
    using Fanex.LiveCasino.CoreSetting.Services;
    using Fanex.LiveCasino.CoreSetting.Utilities;
    using Microsoft.Practices.Unity;

    /// <summary>
    /// Live casino authorization filter.
    /// </summary>
    public class LiveCasinoAuthorizationAttribute : AuthorizeAttribute
    {
        private readonly ICommonUtil _commonUtil = new CommonUtil();

        [Dependency]
        public ILiveCasinoAuthorizationService LiveCasinoAuthenticationService { get; set; }

        public bool IsHost { get; set; }

        /// <summary>
        /// Execute before each action for authorization.
        /// </summary>
        /// <param name="filterContext">Current action context.</param>
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);

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
                    var notAuthorizedUrl = _commonUtil.LiveCasinoSettings.UnAuthorizeUrl;
                    filterContext.Result = new RedirectResult(notAuthorizedUrl);
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
            var settingContext = new SettingContext
            {
                UplineId = Converter.ToInt(GetActionParameterValue(valueProvider, "uplineId")),
                UplineRoleId = Converter.ToInt(GetActionParameterValue(valueProvider, "uplineRoleId")),
                AdminId = Converter.ToInt(GetActionParameterValue(valueProvider, "adminId")),
                TargetCustId = Converter.ToInt(GetActionParameterValue(valueProvider, "targetCustId")),
                TargetRoleId = Converter.ToInt(GetActionParameterValue(valueProvider, "targetRoleId")),
                TargetCustIds = GetActionParameterValue(valueProvider, "targetCustIds")
            };

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
                AdminId = Converter.ToInt(GetActionParameterValue(valueProvider, "settingContext.AdminId")),
                TargetCustId = Converter.ToInt(GetActionParameterValue(valueProvider, "settingContext.TargetCustId")),
                TargetRoleId = Converter.ToInt(GetActionParameterValue(valueProvider, "settingContext.TargetRoleId")),
                TargetCustIds = GetActionParameterValue(valueProvider, "settingContext.TargetCustIds")
            };
            return LiveCasinoAuthenticationService.IsAuthorizedHost(filterContext, settingContext);
        }

        private string GetActionParameterValue(IValueProvider valueProvider, string parameterName)
        {
            return valueProvider.GetValue(parameterName) != null
                                ? valueProvider.GetValue(parameterName).AttemptedValue
                                : string.Empty;
        }
    }
}