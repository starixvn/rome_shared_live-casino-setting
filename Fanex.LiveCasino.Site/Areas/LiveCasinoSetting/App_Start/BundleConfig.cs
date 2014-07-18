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
                .Include("~/areas/LiveCasinoSetting/scripts/libs/angular.js"));
        }

        private static void RegisterStyleSheetBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/areascss/livecasino/css")
                .Include("~/areas/LiveCasinoSetting/content/fanex.livecasino.setting.css"));
        }
    }
}