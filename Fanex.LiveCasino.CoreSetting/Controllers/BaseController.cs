namespace Fanex.LiveCasino.CoreSetting.Controllers
{
    using System.Globalization;
    using System.Threading;
    using System.Web.Mvc;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class BaseController : Controller
    {
        private static readonly CultureInfo DefaultCulture = CultureInfo.CreateSpecificCulture("en-US");
        private static readonly string LanguageKey = "Fanex.LiveCasino.CoreSetting.Language";

        /// <summary>
        /// Set current language.
        /// </summary>
        /// <param name="filterContext">Current filter context.</param>
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var currentLanguage = Session[LanguageKey] != null ? Session[LanguageKey].ToString() : string.Empty;

            if (!string.IsNullOrEmpty(currentLanguage))
            {
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(currentLanguage);
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(currentLanguage);
                Thread.CurrentThread.CurrentCulture.DateTimeFormat = DefaultCulture.DateTimeFormat;
                Thread.CurrentThread.CurrentCulture.NumberFormat = DefaultCulture.NumberFormat;
            }

            base.OnActionExecuting(filterContext);
        }
    }
}