namespace Fanex.LiveCasino.CoreSetting.Services
{
    using System.Collections.Generic;
    using Fanex.LiveCasino.CoreSetting.DTO.Command;
    using Fanex.LiveCasino.CoreSetting.Models;
    using Fanex.NAP.Framework.Data;

    public interface IGameSettingService
    {
        Customer GetCustomerInfo(int custId);

        string GetCustomerSiteByUsername(string username);

        List<GameType> GetGameSettingByCustomerId(int custId);

        List<GameType> GetGameSettingByExchange(int exchangeId, string site);

        List<GameType> GetGameSetting(SettingContext settingContext, Customer custInfo);

        ICommandResult InsertAutoAccount(SettingContext settingContext);

        ICommandResult InsertFromExistingAccount(SettingContext settingContext);

        ICommandResult UpdateLiveCasinoStatus(SettingContext settingContext, bool disableLiveCasino);

        ICommandResults UpdateManual(SettingContext settingContext, Customer custInfo, List<GameType> gameTypes);

        IMultipleCommandResults UpdateManualMultiple(SettingContext settingContext, List<GameType> gameTypes);

        ICommandResult UpdateSyncLiveCasino(SettingContext settingContext, bool isSynced);
    }
}