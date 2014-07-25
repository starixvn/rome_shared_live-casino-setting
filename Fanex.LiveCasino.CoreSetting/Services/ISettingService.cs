namespace Fanex.LiveCasino.CoreSetting.Services
{
    using Fanex.LiveCasino.CoreSetting.Criteria;
    using Fanex.LiveCasino.CoreSetting.Criteria.PtAndCom;
    using Fanex.LiveCasino.CoreSetting.Criteria.WinLimit;
    using Fanex.LiveCasino.CoreSetting.Models;
    using Fanex.LiveCasino.CoreSetting.ViewModels;
    using Fanex.NAP.Framework.Command;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface ISettingService
    {
        ICommandResult UpdateLiveCasinoStatus(UpdateLiveCasinoStatusCriteria criteria);

        string GetSettingJavascriptObjectForUpdate(SettingContext settingContext, string objectName);

        CustInfo GetCustomerInfo(GetCustomerInfoCriteria criteria);

        #region PT and Com

        PtAndComSetting GetSettingForEditSuper(GetSettingForEditSuperCriteria criteria);

        PtAndComSetting GetSettingForEditMaster(GetSettingForEditMasterCriteria criteria);

        PtAndComSetting GetSettingForEditAgent(GetSettingForEditAgentCriteria criteria);

        PtAndComSetting GetSettingForEditMember(GetSettingForEditMemberCriteria criteria);

        PtAndComSetting GetSettingForSuperEditMember(GetSettingForSuperEditMemberCriteria criteria);

        PtAndComSetting GetSettingForMasterEditMember(GetSettingForMasterEditMemberCriteria criteria);

        ICommandResult UpdateSuperPtAndComSetting(SettingContext settingContext, PtAndComSetting ptAndComSetting);

        ICommandResult UpdateMasterPtAndComSetting(SettingContext settingContext, PtAndComSetting ptAndComSetting);

        ICommandResult UpdateAgentPtAndComSetting(SettingContext settingContext, PtAndComSetting ptAndComSetting);

        ICommandResult AgentUpdateMemberPtAndComSetting(SettingContext settingContext, PtAndComSetting ptAndComSetting);

        ICommandResult SuperUpdateMemberPtAndComSetting(SettingContext settingContext, PtAndComSetting ptAndComSetting);

        ICommandResult MasterUpdateMemberPtAndComSetting(SettingContext settingContext, PtAndComSetting ptAndComSetting);

        #endregion

        #region Win Limit

        CurrentMemberWin GetCurrentMemberWinLimit(GetCustomerWinLimitLiveCSCriteria criteria);

        #endregion
    }
}
