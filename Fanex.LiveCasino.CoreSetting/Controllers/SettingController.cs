namespace Fanex.LiveCasino.CoreSetting.Controllers
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Web.Mvc;
    using Fanex.LiveCasino.CoreSetting.Constant;
    using Fanex.LiveCasino.CoreSetting.DTO;
    using Fanex.LiveCasino.CoreSetting.DTO.CommandResult;
    using Fanex.LiveCasino.CoreSetting.Enumerations;
    using Fanex.LiveCasino.CoreSetting.Models;
    using Fanex.LiveCasino.CoreSetting.Services;
    using Fanex.LiveCasino.CoreSetting.Web.Binders;
    using Fanex.LiveCasino.CoreSetting.Web.Filters;

    /// <summary>
    /// Controller handles setting request.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1305:FieldNamesMustNotUseHungarianNotation", Justification = "Reviewed.")]
    public class SettingController : BaseController
    {
        private readonly ISettingService _settingService;

        public SettingController()
            : this(null)
        {
        }

        public SettingController(
            ISettingService settingService)
        {
            _settingService = settingService ?? new SettingService();
        }

        /// <summary>
        /// Edit single customer setting.
        /// </summary>
        /// <param name="settingContext">The setting context.</param>
        /// <returns>
        /// Edit customer page.
        /// </returns>
        [LiveCasinoAuthorization]
        public ActionResult EditSetting(
            [ModelBinder(typeof(LiveCasinoSettingContextModelBinder))]LiveCasinoSettingContext settingContext)
        {
            settingContext.Customer = _settingService.GetCustomer(settingContext);

            return View(ViewPath.EditSettingSinglePage, settingContext);
        }

        /// <summary>
        /// Edit multiple customer.
        /// </summary>
        /// <param name="settingContextMultiple">The setting context multiple.</param>
        /// <returns>
        /// Edit multiple form.
        /// </returns>
        [LiveCasinoAuthorization]
        public ActionResult EditSettingMultiple(
            [ModelBinder(typeof(LiveCasinoSettingContextMultipleModelBinder))] LiveCasinoSettingContextMultiple settingContextMultiple)
        {
            Customer representiveCustomer = _settingService.GetRepresentativeCustIdForEditingMultiple(settingContextMultiple);
            if (representiveCustomer == null)
            {
                return View(ViewPath.NoInformationPage);
            }

            settingContextMultiple.Customer = representiveCustomer;

            return View(ViewPath.EditSettingMultiplePage, settingContextMultiple);
        }

        /// <summary>
        /// Update live casino setting.
        /// </summary>
        /// <param name="settingContext">Setting context for update current customer.</param>
        /// <param name="customer">Customer setting for update data.</param>
        /// <param name="isApplyPositionTakingAllMembers">if set to <c>true</c> [is apply position taking all members].</param>
        /// <param name="positionTakingOnMembers">The position taking on members.</param>
        /// <returns>
        /// Update result information.
        /// </returns>
        [HttpPost, LiveCasinoAuthorization(IsHost = true)]
        public JsonResult UpdateLiveCasinoSetting(
            LiveCasinoSettingContext settingContext,
            Customer customer,
            bool isApplyPositionTakingAllMembers,
            float positionTakingOnMembers)
        {
            IMultipleCommandResultsDTO updateResult = new MultipleCommandResultsDTO();

            updateResult.CommandResultsList.Add(_settingService.UpdateSingleCustomerAllSetting(
                settingContext,
                customer,
                isApplyPositionTakingAllMembers,
                positionTakingOnMembers));

            return Json(updateResult);
        }

        /// <summary>
        /// Updates the live casino setting multiple.
        /// </summary>
        /// <param name="settingContextMultiple">The setting context multiple.</param>
        /// <param name="customer">The customer.</param>
        /// <param name="isApplyPositionTakingAllMembers">if set to <c>true</c> [is apply position taking all members].</param>
        /// <param name="positionTakingOnMembers">The position taking on members.</param>
        /// <param name="updateType">Type of the update.</param>
        /// <returns></returns>
        [HttpPost, LiveCasinoAuthorization(IsHost = true)]
        public JsonResult UpdateLiveCasinoSettingMultiple(
            LiveCasinoSettingContextMultiple settingContextMultiple,
            Customer customer,
            bool isApplyPositionTakingAllMembers,
            float positionTakingOnMembers,
            int updateType = (int)UpdateType.AllSetting)
        {
            IMultipleCommandResultsDTO updateResult = _settingService.UpdateMultipleCustomerAllSetting(
                settingContextMultiple,
                customer,
                updateType,
                isApplyPositionTakingAllMembers,
                positionTakingOnMembers);

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
            CurrentMemberWinLimitDTO currentMemberWinLimit = _settingService.GetCurrentWinLimit(custId);

            return Json(currentMemberWinLimit, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Update win limit.
        /// </summary>
        /// <param name="settingContext">Setting for updating win limit.</param>
        /// <param name="model">Data for update win limit.</param>
        /// <returns>Result after updating win limit.</returns>
        [HttpPost, LiveCasinoAuthorization(IsHost = true)]
        public ActionResult UpdateWinLimit(LiveCasinoSettingContext settingContext, WinLimit model)
        {
            IMultipleCommandResultsDTO updateResult = new MultipleCommandResultsDTO();

            updateResult.CommandResultsList.Add(_settingService.UpdateWinLimit(settingContext, model));

            return Json(updateResult);
        }

        /// <summary>
        /// Updates the win limit multiple.
        /// </summary>
        /// <param name="settingContextMultiple">The setting context multiple.</param>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpPost, LiveCasinoAuthorization(IsHost = true)]
        public ActionResult UpdateWinLimitMultiple(LiveCasinoSettingContextMultiple settingContextMultiple, WinLimit model)
        {
            IMultipleCommandResultsDTO updateResult = new MultipleCommandResultsDTO();

            updateResult = _settingService.UpdateWinLimitMultiple(settingContextMultiple, model);

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
        public ActionResult UpdateGameSetting(LiveCasinoSettingContext settingContext, List<GameType> gameSetting)
        {
            IMultipleCommandResultsDTO updateResult = new MultipleCommandResultsDTO();

            updateResult.CommandResultsList.Add(_settingService.UpdateGameSetting(settingContext, gameSetting));

            return Json(updateResult);
        }

        /// <summary>
        /// Updates the game setting multiple.
        /// </summary>
        /// <param name="settingContextMultiple">The setting context multiple.</param>
        /// <param name="gameSetting">The game setting.</param>
        /// <returns></returns>
        [HttpPost, LiveCasinoAuthorization(IsHost = true)]
        public ActionResult UpdateGameSettingMultiple(LiveCasinoSettingContextMultiple settingContextMultiple, List<GameType> gameSetting)
        {
            IMultipleCommandResultsDTO updateResult = new MultipleCommandResultsDTO();

            updateResult = _settingService.UpdateGameSettingMultiple(settingContextMultiple, gameSetting);

            return Json(updateResult);
        }

        #endregion Game Setting

        #region Pt and Com setting

        /// <summary>
        /// Update position taking and Commission setting.
        /// </summary>
        /// <param name="settingContext">Setting context for updating.</param>
        /// <param name="commission">Commission setting for updating.</param>
        /// <param name="positionTaking">Position taking setting for updating.</param>
        /// <param name="uplinePositionTakingOnMember">Up-line position taking on member when set from auto PT to none.</param>
        /// <returns>Update result information.</returns>
        [HttpPost, LiveCasinoAuthorization(IsHost = true)]
        public JsonResult UpdatePtComSetting(
            LiveCasinoSettingContext settingContext,
            float commission,
            PositionTaking positionTaking,
            bool isApplyPositionTakingAllMembers,
            float positionTakingOnMembers)
        {
            IMultipleCommandResultsDTO updateResult = new MultipleCommandResultsDTO
            {
                CommandResultsList = new List<ICommandResultsDTO>
                {
                    _settingService.UpdatePtComSetting(
                        settingContext,
                        commission,
                        positionTaking,
                        isApplyPositionTakingAllMembers,
                        positionTakingOnMembers)
                }
            };

            return Json(updateResult);
        }

        [HttpPost, LiveCasinoAuthorization(IsHost = true)]
        public JsonResult UpdatePtComSettingMultiple(
            LiveCasinoSettingContextMultiple settingContextMultiple,
            float commission,
            PositionTaking positionTaking,
            bool isApplyPositionTakingAllMembers,
            float positionTakingOnMembers)
        {
            IMultipleCommandResultsDTO updateResult = _settingService.UpdatePtComSettingMultiple(
                settingContextMultiple,
                commission,
                positionTaking,
                isApplyPositionTakingAllMembers,
                positionTakingOnMembers);

            return Json(updateResult);
        }

        #endregion Pt and Com setting
    }
}