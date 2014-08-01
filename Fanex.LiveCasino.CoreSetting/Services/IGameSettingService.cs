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

        ICommandResults InsertAutoGameSetting(SettingContext settingContext, CustInfo custInfo);

        IList<GameType> GetGameSetting(SettingContext settingContext, CustInfo custInfo);
    }
}
