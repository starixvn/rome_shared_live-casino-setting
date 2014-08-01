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
        private readonly IGameSettingService _gameSettingService;

        public LiveCSSettingController()
            : this(null, null)
        {
        }

        public LiveCSSettingController(ISettingService settingService, IGameSettingService gameSettingService)
        {
            _settingService = settingService ?? new SettingService();
            _gameSettingService = gameSettingService ?? new GameSettingService();
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
            int formType = (int)FormType.Edit,
            string language = "E"
            )
        {
            var settingContext = new SettingContext
                {
                    CustId = custId,
                    RoleId = roleId,
                    TargetCustId = targetCustId,
                    TargetRoleId = targetRoleId,
                    FormType = (FormType)formType,
                    AdminId = adminId,
                    Language = language
                };

            return View("~/Areas/LiveCasinoSetting/Views/EditSetting/EditSetting.cshtml", settingContext);
        }

        public ActionResult EditSettingMultiple(
            int custId = 0,
            int roleId = 0,
            string targetCustIds = "",
            string targetUsernames = "",
            string targetSyncStatus = "",
            int targetRoleId = 0,
            int adminId = 0,
            int formType = (int)FormType.Edit,
            string language = "E")
        {

            var settingContext = new SettingContext
            {
                CustId = custId,
                RoleId = roleId,
                TargetRoleId = targetRoleId,
                TargetCustIds = targetCustIds,
                FormType = (FormType)formType,
                AdminId = adminId,
                Language = language,
                IsEditMultiple = true
            };
            return View("~/Areas/LiveCasinoSetting/Views/EditSetting/EditSetting.cshtml", settingContext);
        }
                

        [HttpGet]
        public JavaScriptResult GetSettingForUpdate(SettingContext settingContext)
        {
            settingContext.IP = HttpContext.Request.UserHostAddress;
            return JavaScript(GetSettingJavascriptObjectForUpdate(settingContext, "custSetting"));
        }

        //[InjectedFilter]
        [HttpPost]
        public JsonResult UpdateLiveCasinoSetting(SettingContext settingContext, CustSettingViewModel custSetting)
        {
            //// Check what setting will be update
            ICommandResults resultList = new CommandResults();

            resultList.Results.Add(_gameSettingService.UpdateLiveCasinoStatus(settingContext, custSetting.CustInfo));

            var ptAndCommResults = _settingService.UpdatePtAndComSetting(settingContext, custSetting.PtAndComSetting);
            resultList.Results.AddRange(ptAndCommResults);

            // Only update when PT and Commission updated successfully, waiting for Ricky implement PtAndCommResults
            var winLimitResults = _settingService.UpdateWinLimit(settingContext, custSetting.CustInfo.MemberWinLimit).Results;
            var gameSettingResult = _gameSettingService.UpdateManualGameSetting(settingContext, custSetting.CustInfo, custSetting.GameSetting.ToList()).Results;

            resultList.Results.AddRange(winLimitResults);
            resultList.Results.AddRange(gameSettingResult);

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

            var result = _gameSettingService.UpdateManualGameSetting(settingContext, custInfo, gameSetting);

            return Json(result);
        }

        #endregion

        #region Pt and Com setting

        [HttpPost]
        public JsonResult UpdatePtAndComSetting(SettingContext settingContext, PtAndComSetting ptAndComSetting)
        {
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

        private string GetSettingJavascriptObjectForUpdate(SettingContext settingContext, string objectName)
        {
            CustInfo custInfo = _settingService.GetCustomerInfo(settingContext.TargetCustId);

            var custSetting = new CustSettingViewModel
            {
                SettingContext = settingContext,
                CustInfo = custInfo,
                PtAndComSetting = _settingService.GetPtAndComSetting(settingContext),
                GameSetting = _gameSettingService.GetGameSetting(settingContext, custInfo)
            };

            return FormatJavascript(custSetting, objectName);
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