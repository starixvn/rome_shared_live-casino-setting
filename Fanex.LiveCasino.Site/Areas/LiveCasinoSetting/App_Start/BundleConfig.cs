using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace Fanex.LiveCasino.Site.Areas.LiveCasinoSetting.App_Start
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            RegisterStyleSheetBundles(bundles);
            RegisterJavascriptBundles(bundles);
        }


        private static void RegisterJavascriptBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/areasjs/livecasino/core")
                .Include("~/areas/LiveCasinoSetting/scripts/libs/jquery-2.1.1.js")
                .Include("~/areas/LiveCasinoSetting/scripts/libs/numeral.js")
                .Include("~/areas/LiveCasinoSetting/scripts/libs/angular.js")
                .Include("~/areas/LiveCasinoSetting/scripts/libs/bootstrap.js"));

            bundles.Add(new ScriptBundle("~/areasjs/livecasino/setting")
                .Include("~/areas/LiveCasinoSetting/scripts/core/fanex-live-casino-setting.js")
                .Include("~/areas/LiveCasinoSetting/views/editsetting/scripts/controllers/cust-info-controller.js")
                .Include("~/areas/LiveCasinoSetting/views/editsetting/scripts/controllers/cust-setting-controller.js")
                .Include("~/areas/LiveCasinoSetting/views/editsetting/scripts/controllers/cust-win-limit-controller.js")
                .Include("~/areas/LiveCasinoSetting/views/editsetting/scripts/controllers/pt-and-com-setting-controller.js")
                .Include("~/areas/LiveCasinoSetting/views/editsetting/scripts/directives/pt-and-com-setting-directive.js")
  		        .Include("~/areas/LiveCasinoSetting/views/editsetting/scripts/services/cust-setting-service.js")
                .Include("~/areas/LiveCasinoSetting/views/editsetting/scripts/services/cust-win-limit-service.js")
                .Include("~/areas/LiveCasinoSetting/views/editsetting/scripts/services/pt-and-com-setting-service.js"));

        }

        private static void RegisterStyleSheetBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/areascss/livecasino/css")
                .Include("~/areas/LiveCasinoSetting/content/bootstrap.css")
                .Include("~/areas/LiveCasinoSetting/content/fanex-live-casino-setting.css"));
        }
    }
}