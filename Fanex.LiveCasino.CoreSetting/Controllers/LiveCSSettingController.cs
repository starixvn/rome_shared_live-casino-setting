namespace Fanex.LiveCasino.CoreSetting.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Fanex.LiveCasino.CoreSetting.Enumerations;
    using Fanex.LiveCasino.CoreSetting.Models;
    using Fanex.LiveCasino.CoreSetting.Models.Command;
    using Fanex.LiveCasino.CoreSetting.Services;
    using Fanex.LiveCasino.CoreSetting.ViewModels;

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
            int adminId = 0,
            int formType = (int)FormType.Edit
            )
        {
            var settingContext = new SettingContext
                {
                    CustId = custId,
                    RoleId = roleId,
                    TargetCustId = targetCustId,
                    TargetRoleId = targetRoleId,
                    FormType = (FormType)formType,
                    AdminId = adminId
                };

            return View("~/Areas/LiveCasinoSetting/Views/EditSetting/EditSetting.cshtml", settingContext);
        }

        [HttpGet]
        public JavaScriptResult GetSettingForUpdate(SettingContext settingContext)
        {
            settingContext.IP = HttpContext.Request.UserHostAddress;
            return JavaScript(_settingService.GetSettingJavascriptObjectForUpdate(settingContext, "custSetting"));
        }

        //[InjectedFilter]
        [HttpPost]
        public JsonResult UpdateLiveCasinoSetting(SettingContext settingContext, CustSettingViewModel custSetting)
        {
            settingContext.Language = "E";
            settingContext.IP = HttpContext.Request.UserHostAddress;
            //// Check what setting will be update
            ICommandResults resultList = new CommandResults();

            resultList.Results.Add(_settingService.UpdateLiveCasinoStatus(settingContext, custSetting.CustInfo));
            resultList.Results.AddRange(_settingService.UpdatePtAndComSetting(settingContext, custSetting.PtAndComSetting));

            _settingService.UpdateWinLimit(settingContext, custSetting.CustInfo.MemberWinLimit);

            _settingService.UpdateManualGameSetting(settingContext, custSetting.CustInfo, custSetting.GameSetting.ToList());

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
            ICommandResults result = _settingService.UpdateWinLimit(settingContext, model);

            return Json(result);
        }

        #endregion

        #region Game Setting

        [HttpPost]
        public ActionResult UpdateGameSetting(SettingContext settingContext, List<GameType> gameSetting)
        {
            var custInfo = _settingService.GetCustomerInfo(settingContext.TargetCustId);

            var result = _settingService.UpdateManualGameSetting(settingContext, custInfo, gameSetting);

            return Json(result);
        }

        #endregion

        #region Pt and Com setting

        [HttpPost]
        public JsonResult UpdatePtAndComSetting(SettingContext settingContext, PtAndComSetting ptAndComSetting)
        {
            settingContext.Language = "E";
            settingContext.IP = HttpContext.Request.UserHostAddress;

            ICommandResults updateResults = new CommandResults
                {
                    Results = _settingService.UpdatePtAndComSetting(settingContext, ptAndComSetting).ToList()
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