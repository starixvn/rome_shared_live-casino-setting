namespace Fanex.LiveCasino.CoreSetting.Repositories
{
    using System.Collections.Generic;
    using Fanex.LiveCasino.CoreSetting.Criteria.PtAndCom;
    using Fanex.LiveCasino.CoreSetting.Models;
    using Fanex.NAP.Framework.Command;
    using Fanex.NAP.Framework.Data;

    public interface ISettingRepository : IBaseRepository
    {
        CustInfo GetCustomerInfo(ICriteria criteria);

        PtAndComSetting GetSettingForEditSuper(GetSettingForEditSuperCriteria criteria);

        PtAndComSetting GetSettingForEditMaster(GetSettingForEditMasterCriteria criteria);

        PtAndComSetting GetSettingForEditAgent(GetSettingForEditAgentCriteria criteria);

        PtAndComSetting GetSettingForEditMember(GetSettingForEditMemberCriteria criteria);

        PtAndComSetting GetSettingForSuperEditMember(GetSettingForSuperEditMemberCriteria criteria);

        PtAndComSetting GetSettingForMasterEditMember(GetSettingForMasterEditMemberCriteria criteria);

        ////Dictionary<int, ICommandResult> UpdatePtAndCommSettingMultiple(ICriteria criteria, List<int> custIds);

        List<GameType> GetGameSetting(ICriteria criteria, bool hasChecked);

        ICommandResult InsertGameSettingAccount(ICriteria criteria, ref List<GameType> gameTypes);
    }
}