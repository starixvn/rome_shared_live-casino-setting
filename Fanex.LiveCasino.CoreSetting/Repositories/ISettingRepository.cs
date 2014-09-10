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

        CurrentMemberWin GetCurrentMemberWin(ICriteria criteria);

        object[] GetSettingForEditSuper(ICriteria criteria);

        object[] GetSettingForEditMaster(ICriteria criteria);

        object[] GetSettingForEditAgent(ICriteria criteria);

        object[] GetSettingForEditMember(ICriteria criteria);

        object[] GetSettingForSuperEditMember(ICriteria criteria);

        object[] GetSettingForMasterEditMember(ICriteria criteria);

        List<GameType> GetGameSetting(ICriteria criteria, bool hasCheckedColumn);

        ICommandResult InsertGameSettingAccount(ICriteria criteria, ref List<GameType> gameTypes);
    }
}