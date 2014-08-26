namespace Fanex.LiveCasino.CoreSetting.Controllers
{
    using System.Globalization;
    using System.Threading;
    using System.Web.Mvc;
    using Fanex.LiveCasino.CoreSetting.Utilities;

    /// <summary>
    /// Base controller for handle pre-request.
    /// </summary>
    public class BaseController : Controller
    {
        private readonly CultureInfo _defaultCulture = CultureInfo.CreateSpecificCulture("en-US");
        private readonly ICommonUtil _commonUtil = new CommonUtil();

        /// <summary>
        /// Set current language.
        /// </summary>
        /// <param name="filterContext">Current filter context.</param>
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var currentLanguageKey = _commonUtil.LiveCasinoSettings.LanguageSessionKey;
            var currentLanguage = Session[currentLanguageKey] != null ? Session[currentLanguageKey].ToString() : string.Empty;

            if (!string.IsNullOrEmpty(currentLanguage))
            {
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(currentLanguage);
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(currentLanguage);
                Thread.CurrentThread.CurrentCulture.DateTimeFormat = _defaultCulture.DateTimeFormat;
                Thread.CurrentThread.CurrentCulture.NumberFormat = _defaultCulture.NumberFormat;
            }

            base.OnActionExecuting(filterContext);
        }
    }
}