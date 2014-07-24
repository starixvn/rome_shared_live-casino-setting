using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Microsoft.Practices.Unity;
using Fanex.LiveCasino.CoreSetting.Web.Filters;
using Fanex.LiveCasino.Site.Services;

namespace Fanex.LiveCasino.Site
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            Fanex.Data.Configuration.DbSettingProviderManager.Current.Start();

            //var oldProvider = FilterProviders.Providers.Single(
            //       f => f is FilterAttributeFilterProvider
            //   );
            //FilterProviders.Providers.Remove(oldProvider);

            //var unityBootstrapper = new UnityBootstrapper();
            //unityBootstrapper.RegisterTypes();
            //var provider = new UnityFilterAttributeFilterProvider(unityBootstrapper.ContainerInstance);
            //FilterProviders.Providers.Add(provider);
        }
    }
}