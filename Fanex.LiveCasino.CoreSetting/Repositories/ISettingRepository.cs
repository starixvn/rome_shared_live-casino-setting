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

        PTComSetting GetSettingForEditSuper(GetSettingForEditSuperCriteria criteria);

        PTComSetting GetSettingForEditMaster(GetSettingForEditMasterCriteria criteria);

        PTComSetting GetSettingForEditAgent(GetSettingForEditAgentCriteria criteria);

        PTComSetting GetSettingForEditMember(GetSettingForEditMemberCriteria criteria);

        PTComSetting GetSettingForSuperEditMember(GetSettingForSuperEditMemberCriteria criteria);

        PTComSetting GetSettingForMasterEditMember(GetSettingForMasterEditMemberCriteria criteria);

        List<GameType> GetGameSetting(ICriteria criteria, bool hasCheckedColumn);

        ICommandResult InsertGameSettingAccount(ICriteria criteria, ref List<GameType> gameTypes);
    }
}