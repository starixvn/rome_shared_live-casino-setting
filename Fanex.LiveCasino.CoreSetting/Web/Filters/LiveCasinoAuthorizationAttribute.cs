namespace Fanex.LiveCasino.CoreSetting.Web.Filters
{
    using System.Web.Mvc;
    using Fanex.Common;
    using Fanex.LiveCasino.CoreSetting.Models;
    using Fanex.LiveCasino.CoreSetting.Services;
    using Microsoft.Practices.Unity;
    using System.Web;
    using Fanex.LiveCasino.CoreSetting.Resources;

    /// <summary>
    /// Live casino authorization filter.
    /// </summary>
    public class LiveCasinoAuthorizationAttribute : AuthorizeAttribute
    {
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
                    filterContext.Result = new HttpStatusCodeResult(403, LiveCasinoLang.UnauthorizeMsg);

                    response.Buffer = true;
                    response.StatusCode = 403; // Unauthorized status code
                    response.Write(LiveCasinoLang.UnauthorizeMsg);
                    response.Flush();
                    response.Close();
                    response.End();
                }
                else
                {
                    var unAuthorizedUrl = LiveCasinoAuthenticationService.UnAuthorizedUrl ?? "~/LiveCasinoSetting/LiveCasinoUser/UnAuthorized";
                    filterContext.Result = new RedirectResult(unAuthorizedUrl);
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
                CustId = Converter.ToInt(GetActionParameterValue(valueProvider, "custId")),
                RoleId = Converter.ToInt(GetActionParameterValue(valueProvider, "roleId")),
                AdminId = Converter.ToInt(GetActionParameterValue(valueProvider, "adminId")),
                TargetCustId = Converter.ToInt(GetActionParameterValue(valueProvider, "targetCustId")),
                TargetRoleId = Converter.ToInt(GetActionParameterValue(valueProvider, "targetRoleId")),
                TargetCustIds = GetActionParameterValue(valueProvider, "targetRoleIds")
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
                CustId = Converter.ToInt(GetActionParameterValue(valueProvider, "settingContext.CustId")),
                RoleId = Converter.ToInt(GetActionParameterValue(valueProvider, "settingContext.RoleId")),
                AdminId = Converter.ToInt(GetActionParameterValue(valueProvider, "settingContext.AdminId")),
                TargetCustId = Converter.ToInt(GetActionParameterValue(valueProvider, "settingContext.TargetCustId")),
                TargetRoleId = Converter.ToInt(GetActionParameterValue(valueProvider, "settingContext.TargetRoleId")),
                TargetCustIds = GetActionParameterValue(valueProvider, "settingContext.targetRoleIds")
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