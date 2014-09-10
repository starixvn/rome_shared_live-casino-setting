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

        PTComSetting GetSettingForEditSuper(ICriteria criteria);

        PTComSetting GetSettingForEditMaster(ICriteria criteria);

        PTComSetting GetSettingForEditAgent(ICriteria criteria);

        PTComSetting GetSettingForEditMember(ICriteria criteria);

        PTComSetting GetSettingForSuperEditMember(ICriteria criteria);

        PTComSetting GetSettingForMasterEditMember(ICriteria criteria);

        List<GameType> GetGameSetting(ICriteria criteria, bool hasCheckedColumn);

        ICommandResult InsertGameSettingAccount(ICriteria criteria, ref List<GameType> gameTypes);
    }
}