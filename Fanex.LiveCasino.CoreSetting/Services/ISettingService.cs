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

        ICommandResultsDTO UpdatePtComSetting(
            SettingContext settingContext,
            Commission commission,
            PositionTaking positionTaking,
            UplinePositionTakingOnMember uplinePositionTakingOnMember);

        IMultipleCommandResultsDTO UpdatePtComSettingMultiple(
            SettingContext settingContext,
            Commission commission,
            PositionTaking positionTaking,
            UplinePositionTakingOnMember uplinePositionTakingOnMember);

        Customer GetCustomer(SettingContext settingContext);

        Customer GetCustomerInfo(int custId);

        string GetCustomerSite(string username);      

        List<GameType> GetGameSetting(SettingContext settingContext);

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