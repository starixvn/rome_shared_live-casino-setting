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

        IList<GameType> GetGameSetting(SettingContext settingContext, CustInfo custInfo);        

        ICommandResult InsertAutoAccount(InsertAutoAccountCriteria criteria);

        ICommandResult InsertFromExistingAccount(InsertFromExistingAccountCriteria criteria);

        ICommandResult UpdateLiveCasinoStatus(SettingContext settingContext, CustInfo custInfo);

        ICommandResults UpdateManual(SettingContext settingContext, CustInfo custInfo, List<GameType> gameTypes);

        ICommandResult UpdateSyncLiveCasino(SettingContext settingContext, bool isSync);
    }
}