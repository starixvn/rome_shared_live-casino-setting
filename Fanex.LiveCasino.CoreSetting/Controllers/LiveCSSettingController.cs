// --------------------------------------------------------------------------
// <copyright file="LiveCSSettingController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2014.7.21">Created</change>
// </history>

namespace Fanex.LiveCasino.CoreSetting.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web.Mvc;
    using Fanex.LiveCasino.CoreSetting.Services;
    using Fanex.LiveCasino.CoreSetting.Criteria;
    using Fanex.LiveCasino.CoreSetting.ViewModels;
    using Fanex.LiveCasino.CoreSetting.Models;
    using Fanex.LiveCasino.CoreSetting.Enumerations;
    using Newtonsoft.Json;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class LiveCSSettingController : Controller
    {
        private static JsonSerializerSettings jsonSerializeSettings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore,
            Formatting = Formatting.Indented
        };

        private readonly ISettingService _settingService;
        public LiveCSSettingController() : this(null) { }

        public LiveCSSettingController(ISettingService settingService)
        {
            _settingService = settingService ?? new SettingService();
        }
        //
        // GET: /Setting/

        public ActionResult EditSetting(
            int custId = 0,
            int roleId = 0,
            int targetCustId = 0,
            int targetRoleId = 0
            )
        {
            //// temp customer for update
            targetCustId = 1176083;
            var settingContext = new SettingContext
                {
                    CustId = custId,
                    RoleId = roleId,
                    TargetCustId = targetCustId,
                    TargetRoleId = targetRoleId,
                    FormType = FormType.Edit
                };
            return View("~/Areas/LiveCasinoSetting/Views/EditSetting/EditSetting.cshtml", settingContext);
        }

        [HttpGet]
        public JavaScriptResult GetSettingForUpdate(SettingContext settingContext)
        {
            var custInfo = _settingService.GetCustomerInfo(new GetCustomerInfoCriteria
            {
                Custid = settingContext.TargetCustId
            });

            var custSetting = new CustSettingViewModel
            {
                CustInfo = custInfo,
                SettingContext = settingContext
            };

            return JavaScript(FormatJavascript(custSetting, "custSetting"));
        }

        private string FormatJavascript(object obj, string name)
        {
            if (obj != null)
            {
                return string.Format("var {0} = {1}; \n", name, JsonConvert.SerializeObject(obj, jsonSerializeSettings));
            }

            return string.Empty;
        }
    }
}
