namespace Fanex.LiveCasino.CoreSetting.Repositories
{
    using System.Collections.Generic;
    using Fanex.LiveCasino.CoreSetting.Criteria.PtAndCom;
    using Fanex.LiveCasino.CoreSetting.DTO;
    using Fanex.LiveCasino.CoreSetting.Models;
    using Fanex.NAP.Framework.Data;

    public interface ISettingRepository : IBaseRepository
    {
        Customer GetCustomerInfo(ICriteria criteria);

        CurrentMemberWinLimitDTO GetCurrentMemberWin(ICriteria criteria);

        object[] GetSettingForEditSuper(ICriteria criteria);

        object[] GetSettingForEditMaster(ICriteria criteria);

        object[] GetSettingForEditAgent(ICriteria criteria);

        object[] GetSettingForEditMember(ICriteria criteria);

        object[] GetSettingForSuperEditMember(ICriteria criteria);

        object[] GetSettingForMasterEditMember(ICriteria criteria);

        List<GameType> GetGameSetting(int custId);

        List<GameType> GetGameSetting(int exchangeId, string site, bool defaultCheckedValue = false);

        ICommandResult InsertGameSettingAccount(ICriteria criteria, List<GameType> gameTypes);
    }
}