namespace Fanex.LiveCasino.CoreSetting.Services
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Fanex.LiveCasino.CoreSetting.DTO;
    using Fanex.LiveCasino.CoreSetting.DTO.CommandResult;
    using Fanex.LiveCasino.CoreSetting.Enumerations;
    using Fanex.LiveCasino.CoreSetting.Models;
    using Fanex.NAP.Framework.Data;

    [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1305:FieldNamesMustNotUseHungarianNotation", Justification = "Reviewed.")]
    public interface ISettingService
    {
        int GetRepresentativeCustomerId(LiveCasinoSettingContextMultiple settingContextMultiple);

        Customer GetCustomer(LiveCasinoSettingContext settingContext);

        ICommandResults UpdateCustomerSetting(
            LiveCasinoSettingContext settingContext,
            Customer customer,
            bool isApplyPositionTakingAllMembers = false,
            float positionTakingOnMembers = 0);

        IMultipleCommandResults UpdateMultipleCustomerSetting(
            LiveCasinoSettingContextMultiple settingContextMultiple,
            Customer customer,
            UpdateType updateType,
            bool isApplyPositionTakingAllMembers = false,
            float positionTakingOnMembers = 0);

        /// <summary>
        /// Update position taking and Commission setting, depend on setting context,
        /// we will decide which store procedure and function will be called.
        /// </summary>
        /// <param name="settingContext">Setting context for update customer position taking and commission.</param>
        /// <param name="commission">The commission.</param>
        /// <param name="positionTaking">The position taking.</param>
        /// <param name="isApplyPositionTakingAllMembers">If set to <c>true</c> [is apply position taking all members].</param>
        /// <param name="positionTakingOnMembers">The position taking on members.</param>
        /// <returns>
        /// Update status and information for showing on web.
        /// </returns>
        ICommandResults UpdatePTComSetting(
            LiveCasinoSettingContext settingContext,
            float commission,
            PositionTaking positionTaking,
            bool isApplyPositionTakingAllMembers,
            float positionTakingOnMembers);

        /// <summary>
        /// Update multiple Customer Position Taking and Commission.
        /// </summary>
        /// <param name="settingContextMultiple">The setting context multiple.</param>
        /// <param name="commission">The commission.</param>
        /// <param name="positionTaking">The position taking.</param>
        /// <param name="isApplyPositionTakingAllMembers">If set to <c>true</c> [is apply position taking all members].</param>
        /// <param name="positionTakingOnMembers">The position taking on members.</param>
        /// <returns>
        /// Update status and information for showing on web.
        /// </returns>
        IMultipleCommandResults UpdateMultiplePTComSetting(
            LiveCasinoSettingContextMultiple settingContextMultiple,
            float commission,
            PositionTaking positionTaking,
            bool isApplyPositionTakingAllMembers,
            float positionTakingOnMembers);

        /// <summary>
        /// Automatics insert account to Live Casino.
        /// </summary>
        /// <param name="custId">The customer identifier.</param>
        /// <param name="actorId">The actor identifier.</param>
        /// <param name="actorUsername">The actor username.</param>
        /// <param name="subAccountId">The sub account identifier.</param>
        /// <param name="subAccountUsername">The sub account username.</param>
        /// <param name="ip">The internet protocol.</param>
        /// <param name="language">The language.</param>
        /// <returns>Update result.</returns>
        ICommandResult AutoInsertAccount(
            int custId,
            int actorId,
            string actorUsername,
            int subAccountId,
            string subAccountUsername,
            string ip,
            string language);

        /// <summary>
        /// Automatics insert account to Live Casino from existing account.
        /// </summary>
        /// <param name="custId">The customer identifier.</param>
        /// <param name="oldCustId">The old customer identifier.</param>
        /// <param name="toUsername">To username.</param>
        /// <param name="actorId">The actor identifier.</param>
        /// <param name="actorUsername">The actor username.</param>
        /// <param name="subAccountId">The sub account identifier.</param>
        /// <param name="subAccountUsername">The sub account username.</param>
        /// <param name="ip">The internet protocol.</param>
        /// <param name="language">The language.</param>
        /// <returns>Update result.</returns>
        ICommandResults AutoInsertAccountFromExisting(
            int custId,
            int oldCustId,
            string toUsername,
            int actorId,
            string actorUsername,
            int subAccountId,
            string subAccountUsername,
            string ip,
            string language);

        /// <summary>
        /// Updates the live casino status.
        /// </summary>
        /// <param name="settingContext">The setting context.</param>
        /// <param name="disableLiveCasino">If set to <c>true</c> [disable live casino].</param>
        /// <returns>Update result.</returns>
        ICommandResult UpdateLiveCasinoStatus(LiveCasinoSettingContext settingContext, bool disableLiveCasino);

        /// <summary>
        /// Updates the synchronize live casino.
        /// </summary>
        /// <param name="settingContext">The setting context.</param>
        /// <param name="isSynced">If set to <c>true</c> [is synced].</param>
        /// <returns>Update result.</returns>
        ICommandResult UpdateSyncLiveCasino(LiveCasinoSettingContext settingContext, bool isSynced);

        /// <summary>
        /// Updates the game setting.
        /// </summary>
        /// <param name="settingContext">The setting context.</param>
        /// <param name="gameTypes">The game types.</param>
        /// <returns>Update result.</returns>
        ICommandResults UpdateGameSetting(LiveCasinoSettingContext settingContext, List<GameType> gameTypes);

        /// <summary>
        /// Updates the multiple game setting.
        /// </summary>
        /// <param name="settingContextMultiple">The setting context multiple.</param>
        /// <param name="gameTypes">The game types.</param>
        /// <returns>Update result.</returns>
        IMultipleCommandResults UpdateMultipleGameSetting(LiveCasinoSettingContextMultiple settingContextMultiple, List<GameType> gameTypes);

        /// <summary>
        /// Gets the current win limit.
        /// </summary>
        /// <param name="custId">The customer identifier.</param>
        /// <returns>Current member win limit.</returns>
        CurrentMemberWinLimitDTO GetCurrentWinLimit(int custId);

        /// <summary>
        /// Updates the win limit.
        /// </summary>
        /// <param name="settingContext">The setting context.</param>
        /// <param name="memberWinLimit">The member win limit.</param>
        /// <returns>Update result.</returns>
        ICommandResults UpdateWinLimit(LiveCasinoSettingContext settingContext, WinLimit memberWinLimit);

        /// <summary>
        /// Updates the multiple win limit.
        /// </summary>
        /// <param name="settingContextMultiple">The setting context multiple.</param>
        /// <param name="memberWinLimit">The member win limit.</param>
        /// <returns>Update result.</returns>
        IMultipleCommandResults UpdateMultipleWinLimit(LiveCasinoSettingContextMultiple settingContextMultiple, WinLimit memberWinLimit);
    }
}