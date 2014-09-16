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

        ICommandResultsDTO UpdateCustomerSetting(
            LiveCasinoSettingContext settingContext,
            Customer customer,
            bool isApplyPositionTakingAllMembers,
            float positionTakingOnMembers);

        IMultipleCommandResultsDTO UpdateMultipleCustomerSetting(
            LiveCasinoSettingContextMultiple settingContextMultiple,
            Customer customer,
            UpdateType updateType,
            bool isApplyPositionTakingAllMembers,
            float positionTakingOnMembers);

        /// <summary>
        /// Update position taking and Commission setting, depend on setting context,
        /// we will decide which store procedure and function will be called.
        /// </summary>
        /// <param name="settingContext">Setting context for update customer position taking and commission.</param>
        /// <param name="positionTakingAndComSetting">Position taking and Commission setting for updating.</param>
        /// <returns>Update status and information for showing on web.</returns>
        ICommandResultsDTO UpdatePTComSetting(
            LiveCasinoSettingContext settingContext,
            float commission,
            PositionTaking positionTaking,
            bool isApplyPositionTakingAllMembers,
            float positionTakingOnMembers);

        /// <summary>
        /// Update multiple Customer Position Taking and Commission.
        /// </summary>
        /// <param name="settingContext">Setting context of current update setting.</param>
        /// <param name="positionTakingAndComSetting">Position taking and Commission setting for updating.</param>
        /// <param name="uplinePositionTakingOnMember">Up-line position taking on member when set from auto PT to none.</param>
        /// <returns>Update status and information for showing on web.</returns>
        IMultipleCommandResultsDTO UpdateMultiplePTComSetting(
            LiveCasinoSettingContextMultiple settingContextMultiple,
            float commission,
            PositionTaking positionTaking,
            bool isApplyPositionTakingAllMembers,
            float positionTakingOnMembers);

        Customer GetCustomer(LiveCasinoSettingContext settingContext);

        Customer GetCustomerLiveCasinoInfo(int custId);

        string GetCustomerSite(string username);

        void SetGameSetting(Customer customer);

        ICommandResult InsertAutoAccount(int custId, int actorId, string actorUsername, int subAccountId, string subAccountUsername, string ip, string language);

        ICommandResult InsertFromExistingAccount(LiveCasinoSettingContext settingContext);

        ICommandResult UpdateLiveCasinoStatus(LiveCasinoSettingContext settingContext, bool disableLiveCasino);

        ICommandResultsDTO UpdateGameSetting(LiveCasinoSettingContext settingContext, List<GameType> gameTypes);

        IMultipleCommandResultsDTO UpdateGameSettingMultiple(LiveCasinoSettingContextMultiple settingContextMultiple, List<GameType> gameTypes);

        ICommandResult UpdateSyncLiveCasino(LiveCasinoSettingContext settingContext, bool isSynced);

        CurrentMemberWinLimitDTO GetCurrentWinLimit(int custId);

        ICommandResultsDTO UpdateWinLimit(LiveCasinoSettingContext settingContext, WinLimit memberWinLimit);

        IMultipleCommandResultsDTO UpdateWinLimitMultiple(LiveCasinoSettingContextMultiple settingContextMultiple, WinLimit memberWinLimit);
    }
}