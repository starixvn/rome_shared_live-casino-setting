namespace Fanex.LiveCasino.CoreSetting.Services
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Fanex.LiveCasino.CoreSetting.DTO;
    using Fanex.LiveCasino.CoreSetting.DTO.CommandResult;
    using Fanex.LiveCasino.CoreSetting.Models;
    using Fanex.NAP.Framework.Data;

    [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1305:FieldNamesMustNotUseHungarianNotation", Justification = "Reviewed.")]
    public interface ISettingService
    {
        int GetRepresentativeCustIdForEditingMultiple(SettingContextMultiple settingContextMultiple);

        ICommandResultsDTO UpdateSingleCustomerAllSetting(SettingContext settingContext,
            Customer customer,
            bool isApplyPositionTakingAllMembers,
            float positionTakingOnMembers);

        IMultipleCommandResultsDTO UpdateMultipleCustomerAllSetting(
            SettingContextMultiple settingContextMultiple,
            Customer customer,
            int updateType,
            bool isApplyPositionTakingAllMembers,
            float positionTakingOnMembers);

        /// <summary>
        /// Update position taking and Commission setting, depend on setting context,
        /// we will decide which store procedure and function will be called.
        /// </summary>
        /// <param name="settingContext">Setting context for update customer position taking and commission.</param>
        /// <param name="positionTakingAndComSetting">Position taking and Commission setting for updating.</param>
        /// <returns>Update status and information for showing on web.</returns>
        ICommandResultsDTO UpdatePtComSetting(
            SettingContext settingContext,
            Commission commission,
            PositionTaking positionTaking,
            bool isApplyPositionTakingAllMembers,
            float positionTakingOnMembers);

        /// <summary>
        /// Update multiple Customer Position Taking and Commission.
        /// </summary>
        /// <param name="settingContextMultiple">Setting context of current update setting.</param>
        /// <param name="positionTakingAndComSetting">Position taking and Commission setting for updating.</param>
        /// <param name="uplinePositionTakingOnMember">Up-line position taking on member when set from auto PT to none.</param>
        /// <returns>Update status and information for showing on web.</returns>
        IMultipleCommandResultsDTO UpdatePtComSettingMultiple(
            SettingContextMultiple settingContextMultiple,
            Commission commission,
            PositionTaking positionTaking,
            bool isApplyPositionTakingAllMembers,
            float positionTakingOnMembers);

        Customer GetCustomer(SettingContext settingContext);

        Customer GetCustomerInfo(int custId);

        string GetCustomerSite(string username);

        /// <summary>
        /// Gets the game setting.
        /// </summary>
        /// <param name="settingContext">TargetCustId, CustomerLevel, IsCustomerSyncedLiveCS, TargetUsername, ExchangeId, DirectParentId. </param>                
        /// <returns>The game setting.</returns>
        List<GameType> GetGameSetting(SettingContext settingContext);

        /// <summary>
        /// Inserts the automatic account.
        /// </summary>
        /// <param name="settingContext">AgentId, TargetCustId, ExchangeId, MasterId, ParentUsername, CustomerLevel, ActorId, IP, SuperId, TargetUsername, DirectParentId.</param>
        /// <returns>Insert account command results.</returns>
        ICommandResult InsertAutoAccount(SettingContext settingContext);

        ICommandResult InsertFromExistingAccount(SettingContext settingContext);

        ICommandResult UpdateLiveCasinoStatus(SettingContext settingContext, bool disableLiveCasino);

        ICommandResultsDTO UpdateGameSetting(SettingContext settingContext, List<GameType> gameTypes);

        IMultipleCommandResultsDTO UpdateGameSettingMultiple(SettingContextMultiple settingContextMultiple, List<GameType> gameTypes);

        ICommandResult UpdateSyncLiveCasino(SettingContext settingContext, bool isSynced);

        CurrentMemberWinLimitDTO GetCurrentWinLimit(int custId);

        ICommandResultsDTO UpdateWinLimit(SettingContext settingContext, WinLimit memberWinLimit);

        IMultipleCommandResultsDTO UpdateWinLimitMultiple(SettingContextMultiple settingContextMultiple, WinLimit memberWinLimit);
    }
}