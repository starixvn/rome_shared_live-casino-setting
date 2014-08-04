// --------------------------------------------------------------------------
// <copyright file="InjectedFilterAttribute.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2014.8.4">Created</change>
// </history>

namespace Fanex.LiveCasino.CoreSetting.Web.Filters
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.Practices.Unity;
    using System.Web.Mvc;
    using Fanex.LiveCasino.CoreSetting.Services;
    using Fanex.Common;
    using Fanex.LiveCasino.CoreSetting.Models;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class InjectedFilterAttribute : ActionFilterAttribute
    {
        [Dependency]
        public IAuthenticationService AuthenticationService { get; set; }

        ////public override void OnResultExecuted(ResultExecutedContext filterContext)
        ////{
        ////    if (authenticationService.IsAuthorized())
        ////    {
        ////        filterContext.HttpContext.Response.Write("This Action was authorized!");
        ////    }
        ////}

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.Request.HttpMethod == "POST")
            {
                SettingContext settingContext = filterContext.ActionParameters["settingContext"] as SettingContext;
            }
            else
            {
                var custId = Converter.ToInt(filterContext.RequestContext.HttpContext.Request["custId"]);
                if (!AuthenticationService.IsAuthorized(custId))
                {
                    filterContext.Result = new RedirectResult("~/LiveCasinoSetting/LiveCSSetting/UnAuthorized");
                }
            }

            base.OnActionExecuting(filterContext);
        }
    }
}
