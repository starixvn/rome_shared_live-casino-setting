namespace Fanex.LiveCasino.CoreSetting.Services
{
    using System.Collections.Generic;
    using Fanex.LiveCasino.CoreSetting.Models;
    using Fanex.LiveCasino.CoreSetting.Models.Command;
    using Fanex.NAP.Framework.Command;

    public interface IGameSettingService
    {
        string GetCustomerSiteByUsername(string username);

        ICommandResult UpdateLiveCasinoStatus(SettingContext settingContext, CustInfo custInfo);

        ICommandResult UpdateSyncLiveCasino(SettingContext settingContext, bool isSync);

        ICommandResults UpdateManualGameSetting(SettingContext settingContext, CustInfo custInfo, List<GameType> gameTypes);

        ICommandResults UpdateAutoGameSetting(SettingContext settingContext, CustInfo custInfo, List<GameType> gameTypes);

        List<GameType> GetGameSettingByExchange(int exchangeId, string site);

        List<GameType> GetGameSettingByCustomerId(int custId);

        IList<GameType> GetGameSetting(SettingContext settingContext, CustInfo custInfo);
    }
}
