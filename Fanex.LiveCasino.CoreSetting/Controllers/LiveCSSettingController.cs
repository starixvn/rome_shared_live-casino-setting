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

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class LiveCSSettingController : Controller
    {
        private readonly ISettingService _settingService;
        public LiveCSSettingController() : this(null) { }

        public LiveCSSettingController(ISettingService settingService)
        {
            _settingService = settingService ?? new SettingService();
        }
        //
        // GET: /Setting/

        public ActionResult EditSetting(
            int custId,
            int roleId,
            int targetCustId,
            int targetRoleId
            )
        {
            //// temp customer for update
            targetCustId = 1176083;
            var custInfo = _settingService.GetCustomerInfo(new GetCustomerInfoCriteria
            {
                Custid = targetCustId
            });

            var custSetting = new CustSettingViewModel
            {
                CustInfo = custInfo,
                settingContext = new SettingContext
                {
                    CustId = custId,
                    RoleId = roleId,
                    TargetCustId = targetCustId,
                    TargetRoleId = targetRoleId,
                    FormType = FormType.Edit
                }
            };
            return View("~/Areas/LiveCasinoSetting/Views/EditSetting/EditSetting.cshtml", custSetting);
        }

    }
}
