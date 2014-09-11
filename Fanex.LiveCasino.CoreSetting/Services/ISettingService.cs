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
        int GetRepresentativeCustIdForEditingMultiple(SettingContext settingContext);

        ICommandResultsDTO UpdateSingleCustomerAllSetting(SettingContext settingContext,
            Customer customer,
            UplinePositionTakingOnMember uplinePositionTakingOnMember);

        IMultipleCommandResultsDTO UpdateMultipleCustomerAllSetting(
            SettingContext settingContext,
            Customer customer,
            UplinePositionTakingOnMember uplinePositionTakingOnMember,
            int updateType);

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
            UplinePositionTakingOnMember uplinePositionTakingOnMember);

        /// <summary>
        /// Update multiple Customer Position Taking and Commission.
        /// </summary>
        /// <param name="settingContext">Setting context of current update setting.</param>
        /// <param name="positionTakingAndComSetting">Position taking and Commission setting for updating.</param>
        /// <param name="uplinePositionTakingOnMember">Up-line position taking on member when set from auto PT to none.</param>
        /// <returns>Update status and information for showing on web.</returns>
        IMultipleCommandResultsDTO UpdatePtComSettingMultiple(
            SettingContext settingContext,
            Commission commission,
            PositionTaking positionTaking,
            UplinePositionTakingOnMember uplinePositionTakingOnMember);

        Customer GetCustomer(SettingContext settingContext);

        Customer GetCustomerInfo(int custId);

        string GetCustomerSite(string username);

        /// <summary>
        /// Gets the game setting.
        /// </summary>
        /// <param name="settingContext">TargetCustId, TargetRole, IsTargetSyncedLiveCS, TargetUsername, ExchangeId, DirectUplineId. </param>                
        /// <returns>The game setting.</returns>
        List<GameType> GetGameSetting(SettingContext settingContext);

        /// <summary>
        /// Inserts the automatic account.
        /// </summary>
        /// <param name="settingContext">AgentId, TargetCustId, ExchangeId, MasterId, UplineUsername, TargetRole, UserIdDoAction, IP, SuperId, TargetUsername, DirectUplineId.</param>
        /// <returns>Insert account command results.</returns>
        ICommandResult InsertAutoAccount(SettingContext settingContext);

        ICommandResult InsertFromExistingAccount(SettingContext settingContext);

        ICommandResult UpdateLiveCasinoStatus(SettingContext settingContext, bool disableLiveCasino);

        ICommandResultsDTO UpdateGameSetting(SettingContext settingContext, List<GameType> gameTypes);

        IMultipleCommandResultsDTO UpdateGameSettingMultiple(SettingContext settingContext, List<GameType> gameTypes);

        ICommandResult UpdateSyncLiveCasino(SettingContext settingContext, bool isSynced);

        CurrentMemberWinLimitDTO GetCurrentWinLimit(int custId);

        ICommandResultsDTO UpdateWinLimit(SettingContext settingContext, WinLimit memberWinLimit);

        IMultipleCommandResultsDTO UpdateWinLimitMultiple(SettingContext settingContext, WinLimit memberWinLimit);
    }
}