namespace Fanex.LiveCasino.CoreSetting.Web
{
    using System.Web.Mvc;
    using System.Web.Optimization;
    using Fanex.LiveCasino.CoreSetting.Web.App_Start;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class LiveCasinoSettingAreRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "LiveCasinoSetting";
            }
        }

        /// <summary>
        /// Register admin area.
        /// </summary>
        /// <param name="context">Current context.</param>
        public override void RegisterArea(AreaRegistrationContext context)
        {
            RouteConfig.RegisterRoutes(this.AreaName, context);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}