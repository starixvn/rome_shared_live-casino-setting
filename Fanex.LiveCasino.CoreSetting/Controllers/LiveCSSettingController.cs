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
                TargetUsernames = targetUsernames,
                FormType = (FormType)formType,
                AdminId = adminId,
                Language = language,
                IsEditMultiple = true
            };

            settingContext.TargetCustId = _settingService.GetRepresentativeCustIdForEditingMultiple(targetCustIds, targetSyncStatus);
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

            if (settingContext.IsAgentEdittingMember)
            {
                resultList.Results.Add(_gameSettingService.UpdateLiveCasinoStatus(settingContext, custSetting.CustInfo));
            }

            var ptAndCommResults = _settingService.UpdatePtAndComSetting(settingContext, custSetting.PtAndComSetting);
            resultList.Results.AddRange(ptAndCommResults.Results);

            // Only update when PT and Commission updated successfully, waiting for Ricky implement PtAndCommResults
            var winLimitResults = _settingService.UpdateWinLimit(settingContext, custSetting.CustInfo.MemberWinLimit).Results;
            var gameSettingResult = _gameSettingService.UpdateManual(settingContext, custSetting.CustInfo, custSetting.GameSetting.ToList()).Results;

            resultList.Results.AddRange(winLimitResults);
            resultList.Results.AddRange(gameSettingResult);

            IMultipleCommandResults updateResult = new MultipleCommandResults();

            if (settingContext.IsEditMultiple)
            {

            }
            else
            {
                updateResult.CommandResultsList.Add(resultList);
            }


            return Json(updateResult);
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
            IMultipleCommandResults updateResult = new MultipleCommandResults();

            if (settingContext.IsEditMultiple)
            {
            }
            else
            {
                updateResult.CommandResultsList.Add(_settingService.UpdateWinLimit(settingContext, model));
            }

            return Json(updateResult);
        }

        #endregion

        #region Game Setting

        [HttpPost]
        public ActionResult UpdateGameSetting(SettingContext settingContext, List<GameType> gameSetting)
        {
            var custInfo = _settingService.GetCustomerInfo(settingContext.TargetCustId);

            IMultipleCommandResults updateResult = new MultipleCommandResults();

            if (settingContext.IsEditMultiple)
            {
            }
            else
            {
                updateResult.CommandResultsList.Add(_gameSettingService.UpdateManual(settingContext, custInfo, gameSetting));
            }

            return Json(updateResult);
        }

        #endregion

        #region Pt and Com setting

        [HttpPost]
        public JsonResult UpdatePtAndComSetting(SettingContext settingContext, PtAndComSetting ptAndComSetting)
        {
            IMultipleCommandResults updateResult = new MultipleCommandResults();

            if (settingContext.IsEditMultiple)
            {
                updateResult = _settingService.UpdatePtAndComSettingMultiple(settingContext, ptAndComSetting);
            }
            else
            {
                updateResult.CommandResultsList.Add(_settingService.UpdatePtAndComSetting(settingContext, ptAndComSetting));
            }

            return Json(updateResult);
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
                JsonSerializerSettings settings = new JsonSerializerSettings();
                settings.NullValueHandling = NullValueHandling.Ignore;
                return string.Format("var {0} = {1}; \n", name, JsonConvert.SerializeObject(obj, Formatting.Indented, settings));
            }

            return string.Empty;
        }
    }
}