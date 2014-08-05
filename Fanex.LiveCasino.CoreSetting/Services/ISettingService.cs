namespace Fanex.LiveCasino.CoreSetting.Services
{
    using System.Collections.Generic;
    using Fanex.LiveCasino.CoreSetting.Criteria.PtAndCom;
    using Fanex.LiveCasino.CoreSetting.Models;
    using Fanex.LiveCasino.CoreSetting.Models.Command;
    using Fanex.NAP.Framework.Command;

    public interface ISettingService
    {
        CustInfo GetCustomerInfo(int custId);

        int GetRepresentativeCustIdForEditingMultiple(string custIds, string custSyncStatuses);

        #region PT and Com

        PtAndComSetting GetSettingForEditSuper(GetSettingForEditSuperCriteria criteria);

        PtAndComSetting GetSettingForEditMaster(GetSettingForEditMasterCriteria criteria);

        PtAndComSetting GetSettingForEditAgent(GetSettingForEditAgentCriteria criteria);

        PtAndComSetting GetSettingForEditMember(GetSettingForEditMemberCriteria criteria);

        PtAndComSetting GetSettingForSuperEditMember(GetSettingForSuperEditMemberCriteria criteria);

        PtAndComSetting GetSettingForMasterEditMember(GetSettingForMasterEditMemberCriteria criteria);

        PtAndComSetting GetPtAndComSetting(SettingContext settingContext);

        List<ICommandResult> UpdateSuperPtAndComSetting(SettingContext settingContext, PtAndComSetting ptAndComSetting);

        List<ICommandResult> UpdateMasterPtAndComSetting(SettingContext settingContext, PtAndComSetting ptAndComSetting);

        List<ICommandResult> UpdateAgentPtAndComSetting(SettingContext settingContext, PtAndComSetting ptAndComSetting);

        List<ICommandResult> AgentUpdateMemberPtAndComSetting(SettingContext settingContext, PtAndComSetting ptAndComSetting);

        List<ICommandResult> SuperUpdateMemberPtAndComSetting(SettingContext settingContext, PtAndComSetting ptAndComSetting);

        List<ICommandResult> MasterUpdateMemberPtAndComSetting(SettingContext settingContext, PtAndComSetting ptAndComSetting);

        ICommandResults UpdatePtAndComSetting(SettingContext settingContext, PtAndComSetting ptAndComSetting);

        IMultipleCommandResults UpdatePtAndComSettingMultiple(SettingContext settingContext, PtAndComSetting ptAndComSetting);

        IMultipleCommandResults UpdateMasterPtAndComSettingMultiple(SettingContext settingContext, PtAndComSetting ptAndComSetting);

        IMultipleCommandResults UpdateAgentPtAndComSettingMultiple(SettingContext settingContext, PtAndComSetting ptAndComSetting);

        IMultipleCommandResults AgentUpdateMemberPtAndComSettingMultiple(SettingContext settingContext, PtAndComSetting ptAndComSetting);

        IMultipleCommandResults SuperUpdateMemberPtAndComSettingMultiple(SettingContext settingContext, PtAndComSetting ptAndComSetting);

        IMultipleCommandResults MasterUpdateMemberPtAndComSettingMultiple(SettingContext settingContext, PtAndComSetting ptAndComSetting);

        #endregion PT and Com

        #region Win Limit

        CurrentMemberWin GetCurrentWinLimit(int custId);

        ICommandResults UpdateWinLimit(SettingContext settingContext, MemberWinLimit memberWinLimit);

        #endregion Win Limit
    }
}