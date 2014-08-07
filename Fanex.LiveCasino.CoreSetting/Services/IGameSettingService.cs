namespace Fanex.LiveCasino.CoreSetting.Services
{
    using System.Collections.Generic;
    using Fanex.LiveCasino.CoreSetting.Criteria.GameSetting;
    using Fanex.LiveCasino.CoreSetting.Models;
    using Fanex.LiveCasino.CoreSetting.Models.Command;
    using Fanex.NAP.Framework.Command;

    public interface IGameSettingService
    {
        CustInfo GetCustomerInfo(int custId);

        string GetCustomerSiteByUsername(string username);

        IList<GameType> GetGameSetting(SettingContext settingContext, CustInfo custInfo);

        ICommandResult InsertAutoAccount(SettingContext settingContext);

        ICommandResult InsertFromExistingAccount(SettingContext settingContext);

        ICommandResult UpdateLiveCasinoStatus(SettingContext settingContext, bool disableLiveCasino);

        ICommandResults UpdateManual(SettingContext settingContext, bool isSynced, List<GameType> gameTypes);

        IMultipleCommandResults UpdateManualMultiple(SettingContext settingContext, List<GameType> gameTypes);

        ICommandResult UpdateSyncLiveCasino(SettingContext settingContext, bool isSynced);
    }
}