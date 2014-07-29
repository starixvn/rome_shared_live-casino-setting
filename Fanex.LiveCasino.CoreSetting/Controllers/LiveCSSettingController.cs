namespace Fanex.LiveCasino.CoreSetting.Controllers
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using System.Linq;
    using Fanex.LiveCasino.CoreSetting.Criteria;
    using Fanex.LiveCasino.CoreSetting.Enumerations;
    using Fanex.LiveCasino.CoreSetting.Models;
    using Fanex.LiveCasino.CoreSetting.Services;
    using Fanex.LiveCasino.CoreSetting.ViewModels;
    using Fanex.NAP.Framework.Command;
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
            settingContext.Language = "E";
            settingContext.IP = HttpContext.Request.UserHostAddress;
            //// Check what setting will be update
            ResultList resultList = new ResultList();
            if (custSetting.CustInfo != null)
            {
                resultList.CommandResults.Add(_settingService.UpdateLiveCasinoStatus(settingContext, custSetting.CustInfo));
            }

            resultList.CommandResults.AddRange(_settingService.UpdatePtAndComSetting(settingContext, custSetting.PtAndComSetting));

            return Json(resultList);
        }

        #region Win Limit

        [HttpGet]
        public ActionResult GetCurrentWinLimit(int custId)
        {
            CurrentMemberWin currentMemberWinLimit = _settingService.GetCurrentWinLimit(custId);

            return Json(currentMemberWinLimit, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult UpdateWinLimit(SettingContext settingContext, MemberWinLimit model)
        {
            ICommandResult result = _settingService.UpdateWinLimit(settingContext, model);

            return Json(result);
        }

        #endregion

        #region Game Setting

        [HttpPost]
        public ActionResult UpdateGameSetting(SettingContext settingContext, List<GameType> gameSetting)
        {
            return Json(gameSetting);
        }

        #endregion

        #region Pt and Com setting
            
        [HttpPost]
        public JsonResult UpdatePtAndComSetting(SettingContext settingContext, PtAndComSetting ptAndComSetting)
        {
            settingContext.Language = "E";
            settingContext.IP = HttpContext.Request.UserHostAddress;

            ResultList updateResults = new ResultList
                {
                    CommandResults = _settingService.UpdatePtAndComSetting(settingContext, ptAndComSetting).ToList()
                };

            return Json(updateResults);
        }

        #endregion

        public ActionResult UnAuthorized()
        {
            return View("~/Areas/LiveCasinoSetting/Views/Common/UnAuthorized.cshtml");
        }

    }
}