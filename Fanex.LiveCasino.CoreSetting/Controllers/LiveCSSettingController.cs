namespace Fanex.LiveCasino.CoreSetting.Controllers
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Fanex.LiveCasino.CoreSetting.Criteria;
    using Fanex.LiveCasino.CoreSetting.Enumerations;
    using Fanex.LiveCasino.CoreSetting.Models;
    using Fanex.LiveCasino.CoreSetting.Services;
    using Fanex.LiveCasino.CoreSetting.ViewModels;
    using Fanex.NAP.Framework.Command;
    using Fanex.LiveCasino.CoreSetting.Web.Filters;
    using Fanex.LiveCasino.CoreSetting.Criteria.WinLimit;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class LiveCSSettingController : Controller
    {
        private readonly ISettingService _settingService;

        public LiveCSSettingController()
            : this(null)
        {
        }

        public LiveCSSettingController(ISettingService settingService)
        {
            _settingService = settingService ?? new SettingService();
        }

        //
        // GET: /Setting/
        //[InjectedFilter]
        public ActionResult EditSetting(
            int custId = 0,
            int roleId = 0,
            int targetCustId = 0,
            int targetRoleId = 0,
            int adminId = 0
            )
        {
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
            return JavaScript(_settingService.GetSettingJavascriptObjectForUpdate(settingContext, "custSetting"));
        }

        //[InjectedFilter]
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

        [HttpGet]
        public ActionResult GetCurrentMemberWinLimit(int custId)
        {
            CurrentMemberWin currentMemberWinLimit = _settingService.GetCurrentMemberWinLimit(new GetCustomerWinLimitLiveCSCriteria
            {
                CustId = custId
            });
	[HttpGet]
        public ActionResult GetCurrentMemberWinLimit(int custId)
        {
            CurrentMemberWin currentMemberWinLimit = _settingService.GetCurrentMemberWinLimit(new GetCustomerWinLimitLiveCSCriteria
            {
                CustId = custId
            });

            return Json(currentMemberWinLimit, JsonRequestBehavior.AllowGet);
        }


        public ActionResult UnAuthorized()
        {
            return View("~/Areas/LiveCasinoSetting/Views/Common/UnAuthorized.cshtml");
        }
    }
}