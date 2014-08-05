namespace Fanex.LiveCasino.CoreSetting.Services
{
    using System.Collections.Generic;
    using Fanex.LiveCasino.CoreSetting.Criteria.GameSetting;
    using Fanex.LiveCasino.CoreSetting.Models;
    using Fanex.LiveCasino.CoreSetting.Models.Command;
    using Fanex.NAP.Framework.Command;

    public interface IGameSettingService
    {
        string GetCustomerSiteByUsername(string username);

        ICommandResult UpdateLiveCasinoStatus(SettingContext settingContext, CustInfo custInfo);

        ICommandResult UpdateSyncLiveCasino(SettingContext settingContext, bool isSync);

        ICommandResults UpdateManual(SettingContext settingContext, CustInfo custInfo, List<GameType> gameTypes);

        ICommandResult InsertAutoAccount(InsertAutoAccountCriteria criteria);

        ICommandResult InsertFromExistingAccount(InsertFromExistingAccountCriteria criteria);

        IList<GameType> GetGameSetting(SettingContext settingContext, CustInfo custInfo);
    }
}
