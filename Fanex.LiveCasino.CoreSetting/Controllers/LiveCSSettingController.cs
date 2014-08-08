namespace Fanex.LiveCasino.CoreSetting.Controllers
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Web.Mvc;
    using Fanex.LiveCasino.CoreSetting.Enumerations;
    using Fanex.LiveCasino.CoreSetting.Models;
    using Fanex.LiveCasino.CoreSetting.Models.Command;
    using Fanex.LiveCasino.CoreSetting.Services;
    using Fanex.LiveCasino.CoreSetting.Utilities;
    using Fanex.LiveCasino.CoreSetting.ViewModels;
    using Fanex.LiveCasino.CoreSetting.Web.Filters;
    using Newtonsoft.Json;

    /// <summary>
    /// Controller handles setting request.
    /// </summary>
    public class LiveCSSettingController : BaseController
    {
        private readonly ISettingService _settingService;
        private readonly IGameSettingService _gameSettingService;
        private readonly ICommonUtil _commonUtil;

        public LiveCSSettingController()
            : this(null, null, null)
        {
        }

        public LiveCSSettingController(ISettingService settingService, IGameSettingService gameSettingService, ICommonUtil commonUtil)
        {
            _settingService = settingService ?? new SettingService();
            _gameSettingService = gameSettingService ?? new GameSettingService();
            _commonUtil = commonUtil ?? new CommonUtil();
        }

        /// <summary>
        /// Edit single customer setting.
        /// </summary>
        /// <param name="custId">Customer Id is doing action.</param>
        /// <param name="directUplineId">The direct upline identifier.</param>
        /// <param name="roleId">Role id of current customer doing action.</param>
        /// <param name="targetCustId">Target customer is being edited.</param>
        /// <param name="targetRoleId">Target role of customer being edited.</param>
        /// <param name="adminId">Admin id doing action.</param>
        /// <param name="formType">Current Form.</param>
        /// <param name="language">Current language key.</param>
        /// <returns>
        /// Edit customer page.
        /// </returns>
        [LiveCasinoAuthorization]
        public ActionResult EditSetting(
            int custId = 0,
            int directUplineId = 0,
            int roleId = 0,
            int targetCustId = 0,
            int targetRoleId = 0,
            int adminId = 0,
            int formType = (int)FormType.Edit,
            string language = "E")
        {
            var settingContext = new SettingContext
                {
                    CustId = custId,
                    DirectUplineId = directUplineId,
                    RoleId = roleId,
                    TargetCustId = targetCustId,
                    TargetRoleId = targetRoleId,
                    FormType = (FormType)formType,
                    AdminId = adminId,
                    Language = language
                };

            return View("~/Areas/LiveCasinoSetting/Views/EditSetting/EditSetting.cshtml", settingContext);
        }

        /// <summary>
        /// Edit multiple customer.
        /// </summary>
        /// <param name="custId">Customer id is doing edit.</param>
        /// <param name="roleId">Customer role id of user is doing edit.</param>
        /// <param name="targetCustIds">List of customer id are being edited.</param>
        /// <param name="targetUsernames">List of customer name are being edited.</param>
        /// <param name="targetSyncStatus">List of sync customer status are being edited.</param>
        /// <param name="targetRoleId">Target customer role id of Customer is being edited.</param>
        /// <param name="adminId">Admin id is doing action.</param>
        /// <param name="formType">Current form type.</param>
        /// <param name="language">Current language.</param>
        /// <returns>Edit multiple form.</returns>
        [LiveCasinoAuthorization]
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
                IsEditMultiple = true,
                TargetSyncStatus = targetSyncStatus
            };

            settingContext.TargetCustId = _settingService.GetRepresentativeCustIdForEditingMultiple(settingContext);
            if (settingContext.TargetCustId == 0)
            {
                return View("~/Areas/LiveCasinoSetting/Views/Common/NoInformation.cshtml");
            }

            return View("~/Areas/LiveCasinoSetting/Views/EditSetting/EditSetting.cshtml", settingContext);
        }

        /// <summary>
        /// Get JAVASCRIPT string for editing customer.
        /// </summary>
        /// <param name="settingContext">Current setting context for loading data.</param>
        /// <returns>JAVASCRIPT string.</returns>
        [HttpGet, LiveCasinoAuthorization]
        public JavaScriptResult GetSettingForUpdate(SettingContext settingContext)
        {
            settingContext.IP = HttpContext.Request.UserHostAddress;
            return JavaScript(GetSettingJavascriptObjectForUpdate(settingContext, "custSetting"));
        }

        /// <summary>
        /// Update live casino setting.
        /// </summary>
        /// <param name="settingContext">Setting context for update current customer.</param>
        /// <param name="custSetting">Customer setting for update data.</param>
        /// <returns>Update result information.</returns>
        [HttpPost, LiveCasinoAuthorization(IsHost = true)]
        public JsonResult UpdateLiveCasinoSetting(SettingContext settingContext, CustSettingViewModel custSetting)
        {
            IMultipleCommandResults updateResult = new MultipleCommandResults();

            if (settingContext.IsEditMultiple)
            {
                updateResult = UpdateMultipleCustomerAllSetting(settingContext, custSetting);
            }
            else
            {
                updateResult.CommandResultsList.Add(UpdateSingleCustomerAllSetting(settingContext, custSetting));
            }

            return Json(updateResult);
        }

        #region Win Limit

        /// <summary>
        /// Get current win limit for updating.
        /// </summary>
        /// <param name="custId">Customer id for get data.</param>
        /// <returns>JSON object for returning data.</returns>
        [HttpGet, LiveCasinoAuthorization]
        public ActionResult GetCurrentWinLimit(int custId)
        {
            CurrentMemberWin currentMemberWinLimit = _settingService.GetCurrentWinLimit(custId);

            return Json(currentMemberWinLimit, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Update win limit.
        /// </summary>
        /// <param name="settingContext">Setting for updating win limit.</param>
        /// <param name="model">Data for update win limit.</param>
        /// <returns>Result after updating win limit.</returns>
        [HttpPost]
        public ActionResult UpdateWinLimit(SettingContext settingContext, MemberWinLimit model)
        {
            IMultipleCommandResults updateResult = new MultipleCommandResults();

            if (settingContext.IsEditMultiple)
            {
                updateResult = _settingService.UpdateWinLimitMultiple(settingContext, model);
            }
            else
            {
                updateResult.CommandResultsList.Add(_settingService.UpdateWinLimit(settingContext, model));
            }

            return Json(updateResult);
        }

        #endregion Win Limit

        #region Game Setting

        /// <summary>
        /// Update game setting information.
        /// </summary>
        /// <param name="settingContext">Setting context for updating.</param>
        /// <param name="gameSetting">Game setting information for updating.</param>
        /// <returns>Update result information.</returns>
        [HttpPost, LiveCasinoAuthorization(IsHost = true)]
        public ActionResult UpdateGameSetting(SettingContext settingContext, List<GameType> gameSetting)
        {
            var custInfo = _gameSettingService.GetCustomerInfo(settingContext.TargetCustId);

            IMultipleCommandResults updateResult = new MultipleCommandResults();

            if (settingContext.IsEditMultiple)
            {
                updateResult = _gameSettingService.UpdateManualMultiple(settingContext, gameSetting);
            }
            else
            {
                updateResult.CommandResultsList.Add(_gameSettingService.UpdateManual(settingContext, custInfo.IsSyncLiveCasino, gameSetting));
            }

            return Json(updateResult);
        }

        #endregion Game Setting

        #region Pt and Com setting

        /// <summary>
        /// Update position taking and commission setting.
        /// </summary>
        /// <param name="settingContext">Setting context for updating.</param>
        /// <param name="ptAndComSetting">Position taking and commission setting for updating.</param>
        /// <returns>Update result information.</returns>
        [HttpPost, LiveCasinoAuthorization(IsHost = true)]
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1305:FieldNamesMustNotUseHungarianNotation", Justification = "Reviewed.")]
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

        #endregion Pt and Com setting

        /// <summary>
        /// Update single customer all setting.
        /// </summary>
        /// <param name="settingContext">Current setting context.</param>
        /// <param name="custSetting">Customer setting information for updating.</param>
        /// <returns>Update result.</returns>
        private ICommandResults UpdateSingleCustomerAllSetting(SettingContext settingContext, CustSettingViewModel custSetting)
        {
            ICommandResults resultList = new CommandResults();

            if (settingContext.IsAgentEditingMember)
            {
                resultList.Results.Add(_gameSettingService.UpdateLiveCasinoStatus(settingContext, custSetting.CustInfo.DisableLiveCasino));
            }

            var positionTakingAndCommResults = _settingService.UpdatePtAndComSetting(settingContext, custSetting.PtAndComSetting);
            resultList.Results.AddRange(positionTakingAndCommResults.Results);

            if (positionTakingAndCommResults.IsSuccessful)
            {
                var winLimitResults = _settingService.UpdateWinLimit(settingContext, custSetting.CustInfo.MemberWinLimit).Results;
                resultList.Results.AddRange(winLimitResults);

                if (custSetting.GameSetting != null)
                {
                    var gameSettingResult = _gameSettingService.UpdateManual(settingContext, custSetting.CustInfo.IsSyncLiveCasino, custSetting.GameSetting.ToList()).Results;
                    resultList.Results.AddRange(gameSettingResult);
                }
            }
            return resultList;
        }

        /// <summary>
        /// Update multiple customer all settting.
        /// </summary>
        /// <param name="settingContext">Setting context for updating.</param>
        /// <param name="custSetting">Cust setting contains customer setting information.</param>
        /// <returns>Multiple result return to user.</returns>
        private IMultipleCommandResults UpdateMultipleCustomerAllSetting(SettingContext settingContext, CustSettingViewModel custSetting)
        {
            var custInfoes = _commonUtil.GetCustomerInfo(settingContext.TargetCustIds, settingContext.TargetUsernames, settingContext.TargetSyncStatus);

            IMultipleCommandResults mainResults = new MultipleCommandResults();

            foreach (var custInfo in custInfoes)
            {
                settingContext.TargetCustId = custInfo.CustId;
                settingContext.TargetUsername = custInfo.UserName;
                custSetting.CustInfo.IsSyncLiveCasino = custInfo.IsSyncLiveCasino;
                var subResult = UpdateSingleCustomerAllSetting(settingContext, custSetting);
                subResult.Id = custInfo.CustId;
                subResult.Name = custInfo.UserName;
                mainResults.CommandResultsList.Add(subResult);
            }

            return mainResults;
        }

        /// <summary>
        /// Get setting object for showing edit form.
        /// </summary>
        /// <param name="settingContext">Setting context for get setting.</param>
        /// <param name="objectName">JAVASCRIPT object name.</param>
        /// <returns>JAVASCRIPT object string for returning to client.</returns>
        private string GetSettingJavascriptObjectForUpdate(SettingContext settingContext, string objectName)
        {
            CustInfo custInfo = _gameSettingService.GetCustomerInfo(settingContext.TargetCustId);

            var custSetting = new CustSettingViewModel
            {
                SettingContext = settingContext,
                CustInfo = custInfo,
                PtAndComSetting = _settingService.GetPtAndComSetting(settingContext),
                GameSetting = _gameSettingService.GetGameSetting(settingContext, custInfo)
            };

            return FormatJavascript(custSetting, objectName);
        }

        /// <summary>
        /// Format object to JAVASCRIPT string.
        /// </summary>
        /// <param name="obj">Object need be formatted.</param>
        /// <param name="name">Name of object.</param>
        /// <returns>JAVASCRIPT string.</returns>
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