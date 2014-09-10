namespace Fanex.LiveCasino.CoreSetting.Controllers
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Web.Mvc;
    using Fanex.LiveCasino.CoreSetting.Configuration;
    using Fanex.LiveCasino.CoreSetting.Constant;
    using Fanex.LiveCasino.CoreSetting.DTO;
    using Fanex.LiveCasino.CoreSetting.DTO.Command;
    using Fanex.LiveCasino.CoreSetting.Enumerations;
    using Fanex.LiveCasino.CoreSetting.Models;
    using Fanex.LiveCasino.CoreSetting.Services;
    using Fanex.LiveCasino.CoreSetting.ViewModels;
    using Fanex.LiveCasino.CoreSetting.Web.Filters;
    using Newtonsoft.Json;

    /// <summary>
    /// Controller handles setting request.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1305:FieldNamesMustNotUseHungarianNotation", Justification = "Reviewed.")]
    public class LiveCSSettingController : BaseController
    {
        private readonly ISettingService _settingService;
        private readonly IGameSettingService _gameSettingService;
        private readonly IPTComSettingService _pTComSettingService;
        private readonly IWinLimitSettingService _winLimitSettingService;

        public LiveCSSettingController()
            : this(null, null, null, null)
        {
        }

        public LiveCSSettingController(
            ISettingService settingService,
            IPTComSettingService pTComSettingService,
            IWinLimitSettingService winLimitSettingService,
            IGameSettingService gameSettingService)
        {
            _settingService = settingService ?? new SettingService();
            _pTComSettingService = pTComSettingService ?? new PTComSettingService();
            _gameSettingService = gameSettingService ?? new GameSettingService();
            _winLimitSettingService = winLimitSettingService ?? new WinLimitSettingService();
        }

        /// <summary>
        /// Edit single customer setting.
        /// </summary>
        /// <param name="uplineId">Customer Id is doing action.</param>
        /// <param name="directUplineId">The direct UPLINE identifier.</param>
        /// <param name="uplineRoleId">Role id of current customer doing action.</param>
        /// <param name="targetCustId">Target customer is being edited.</param>
        /// <param name="targetRoleId">Target role of customer being edited.</param>
        /// <param name="adminId">Admin id doing action.</param>
        /// <param name="subAccId">Sub account id for doing action.</param>
        /// <param name="formType">Current Form.</param>
        /// <param name="language">Current language key.</param>
        /// <returns>
        /// Edit customer page.
        /// </returns>
        [LiveCasinoAuthorization]
        public ActionResult EditSetting(
            int uplineId = 0,
            int directUplineId = 0,
            int uplineRoleId = 0,
            int targetCustId = 0,
            int targetRoleId = 0,
            int adminId = 0,
            int subAccId = 0,
            int formType = (int)FormType.Edit,
            string language = "E")
        {
            var settingContext = new SettingContext
                {
                    UplineId = uplineId,
                    DirectUplineId = directUplineId,
                    UplineRoleId = uplineRoleId,
                    TargetCustId = targetCustId,
                    TargetRoleId = targetRoleId,
                    FormType = (FormType)formType,
                    AdminId = adminId,
                    Language = language,
                    SubAccId = subAccId,
                    SystemId = LiveCasinoSettings.Settings.SystemId,
                    LangKey = GetLanguageKey()
                };

            SetCustSettingJavascriptObjectToViewBag(settingContext);

            return View(ViewPath.EditSettingPage, settingContext);
        }

        /// <summary>
        /// Edit multiple customer.
        /// </summary>
        /// <param name="uplineId">Customer id is doing edit.</param>
        /// <param name="directUplineId">Customer direct up-line id is doing edit.</param>
        /// <param name="uplineRoleId">Customer role id of user is doing edit.</param>
        /// <param name="targetCustIds">List of customer id are being edited.</param>
        /// <param name="targetUsernames">List of customer name are being edited.</param>
        /// <param name="targetSyncStatus">List of sync customer status are being edited.</param>
        /// <param name="targetRoleId">Target customer role id of Customer is being edited.</param>
        /// <param name="adminId">Admin id is doing action.</param>
        /// <param name="subAccId">Sub account id for doing action.</param>
        /// <param name="formType">Current form type.</param>
        /// <param name="language">Current language.</param>
        /// <returns>
        /// Edit multiple form.
        /// </returns>
        [LiveCasinoAuthorization]
        public ActionResult EditSettingMultiple(
            int uplineId = 0,
            int directUplineId = 0,
            int uplineRoleId = 0,
            string targetCustIds = "",
            string targetUsernames = "",
            string targetSyncStatus = "",
            int targetRoleId = 0,
            int adminId = 0,
            int subAccId = 0,
            int formType = (int)FormType.Edit,
            string language = "E")
        {
            var settingContext = new SettingContext
            {
                UplineId = uplineId,
                DirectUplineId = directUplineId,
                UplineRoleId = uplineRoleId,
                TargetRoleId = targetRoleId,
                TargetCustIds = targetCustIds,
                TargetUsernames = targetUsernames,
                FormType = (FormType)formType,
                AdminId = adminId,
                Language = language,
                IsEditMultiple = true,
                SubAccId = subAccId,
                TargetSyncStatus = targetSyncStatus,
                SystemId = LiveCasinoSettings.Settings.SystemId,
                LangKey = GetLanguageKey()
            };

            settingContext.TargetCustId = _settingService.GetRepresentativeCustIdForEditingMultiple(settingContext);
            if (settingContext.TargetCustId == 0)
            {
                return View(ViewPath.NoInformationPage);
            }

            SetCustSettingJavascriptObjectToViewBag(settingContext);

            return View(ViewPath.EditSettingPage, settingContext);
        }

        /// <summary>
        /// Update live casino setting.
        /// </summary>
        /// <param name="settingContext">Setting context for update current customer.</param>
        /// <param name="customer">Customer setting for update data.</param>
        /// <param name="updateType">Update type for multiple.</param>
        /// <returns>Update result information.</returns>
        [HttpPost, LiveCasinoAuthorization(IsHost = true)]
        public JsonResult UpdateLiveCasinoSetting(SettingContext settingContext, Customer customer, int updateType = (int)UpdateType.AllSetting)
        {
            IMultipleCommandResults updateResult = new MultipleCommandResults();

            if (settingContext.IsEditMultiple)
            {
                updateResult = _settingService.UpdateMultipleCustomerAllSetting(settingContext, customer, updateType);
            }
            else
            {
                updateResult.CommandResultsList.Add(_settingService.UpdateSingleCustomerAllSetting(settingContext, customer));
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
            CurrentMemberWin currentMemberWinLimit = _winLimitSettingService.GetCurrentWinLimit(custId);

            return Json(currentMemberWinLimit, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Update win limit.
        /// </summary>
        /// <param name="settingContext">Setting for updating win limit.</param>
        /// <param name="model">Data for update win limit.</param>
        /// <returns>Result after updating win limit.</returns>
        [HttpPost, LiveCasinoAuthorization(IsHost = true)]
        public ActionResult UpdateWinLimit(SettingContext settingContext, MemberWinLimit model)
        {
            IMultipleCommandResults updateResult = new MultipleCommandResults();

            if (settingContext.IsEditMultiple)
            {
                updateResult = _winLimitSettingService.UpdateWinLimitMultiple(settingContext, model);
            }
            else
            {
                updateResult.CommandResultsList.Add(_winLimitSettingService.UpdateWinLimit(settingContext, model));
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
                updateResult.CommandResultsList.Add(_gameSettingService.UpdateManual(settingContext, custInfo, gameSetting));
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
        public JsonResult UpdatePtAndComSetting(SettingContext settingContext, PTComSetting ptAndComSetting)
        {
            IMultipleCommandResults updateResult = new MultipleCommandResults();

            if (settingContext.IsEditMultiple)
            {
                updateResult = _pTComSettingService.UpdatePtAndComSettingMultiple(settingContext, ptAndComSetting);
            }
            else
            {
                updateResult.CommandResultsList.Add(_pTComSettingService.UpdatePtAndComSetting(settingContext, ptAndComSetting));
            }

            return Json(updateResult);
        }

        #endregion Pt and Com setting

        /// <summary>
        /// Get setting object for showing edit form.
        /// </summary>
        /// <param name="settingContext">Setting context for get setting.</param>
        /// <param name="objectName">JAVASCRIPT object name.</param>
        /// <returns>JAVASCRIPT object string for returning to client.</returns>
        private string GetSettingJavascriptObjectForUpdate(SettingContext settingContext, string objectName)
        {
            var custSettingViewModel = new CustSettingViewModel
            {
                SettingContext = settingContext,
                Customer = _settingService.GetCustomer(settingContext)
            };

            return FormatJavascript(custSettingViewModel, objectName);
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

        private void SetCustSettingJavascriptObjectToViewBag(SettingContext settingContext)
        {
            ViewBag.CustSettingViewModel = GetSettingJavascriptObjectForUpdate(settingContext, "custSetting");
        }

        private string GetLanguageKey()
        {
            return HttpContext.Session[LiveCasinoSettings.Settings.LanguageSessionKey] != null ?
                HttpContext.Session[LiveCasinoSettings.Settings.LanguageSessionKey].ToString() :
                "en-US";
        }
    }
}