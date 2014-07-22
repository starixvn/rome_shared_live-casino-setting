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
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Fanex.LiveCasino.CoreSetting.Criteria;
    using Fanex.LiveCasino.CoreSetting.Enumerations;
    using Fanex.LiveCasino.CoreSetting.Models;
    using Fanex.LiveCasino.CoreSetting.Services;
    using Fanex.LiveCasino.CoreSetting.ViewModels;
    using Newtonsoft.Json;
    using Fanex.NAP.Framework.Command;

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
            int targetRoleId = 0,
            int adminId = 0
            )
        {
            //// temp customer for update
            //targetCustId = 1176083;
            targetCustId = 14657007;
            targetRoleId = (int)CustRole.Member;
            adminId = 8;
            var settingContext = new SettingContext
                {
                    CustId = custId,
                    RoleId = roleId,
                    TargetCustId = targetCustId,
                    TargetRoleId = targetRoleId,
                    FormType = FormType.Edit,
                    AdminId = adminId
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

        [HttpPost]
        public JsonResult UpdateLiveCasinoSetting(SettingContext settingContext, CustSettingViewModel custSetting)
        {
            //// Check what setting will be update
            List<ICommandResult> results = new List<ICommandResult>();
            if (custSetting.CustInfo != null)
            {
                results.Add(_settingService.UpdateLiveCasinoStatus(new UpdateLiveCasinoStatusCriteria
                {
                    Custid = settingContext.TargetCustId,
                    UserID = settingContext.AdminId > 0 ? settingContext.AdminId : settingContext.CustId,
                    DisabledLiveCS = custSetting.CustInfo.DisableLiveCasino,
                    IP = HttpContext.Request.UserHostAddress,
                    Language = "E"
                }));
            }

            return Json(results);
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
