namespace Fanex.LiveCasino.CoreSetting.Services
{
    using Fanex.LiveCasino.CoreSetting.Criteria;
    using Fanex.LiveCasino.CoreSetting.Criteria.PtAndCom;
    using Fanex.LiveCasino.CoreSetting.Criteria.WinLimit;
    using Fanex.LiveCasino.CoreSetting.Models;
    using Fanex.LiveCasino.CoreSetting.ViewModels;
    using Fanex.NAP.Framework.Command;
    using System.Collections.Generic;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface ISettingService
    {
        ICommandResult UpdateLiveCasinoStatus(UpdateLiveCasinoStatusCriteria criteria);

        string GetSettingJavascriptObjectForUpdate(SettingContext settingContext, string objectName);

        CustInfo GetCustomerInfo(GetCustomerInfoCriteria criteria);

        string GetCustomerSiteByUsername(string username);

        #region PT and Com

        PtAndComSetting GetSettingForEditSuper(GetSettingForEditSuperCriteria criteria);

        PtAndComSetting GetSettingForEditMaster(GetSettingForEditMasterCriteria criteria);

        PtAndComSetting GetSettingForEditAgent(GetSettingForEditAgentCriteria criteria);

        PtAndComSetting GetSettingForEditMember(GetSettingForEditMemberCriteria criteria);

        PtAndComSetting GetSettingForSuperEditMember(GetSettingForSuperEditMemberCriteria criteria);

        PtAndComSetting GetSettingForMasterEditMember(GetSettingForMasterEditMemberCriteria criteria);

        IList<CommandResult> UpdateSuperPtAndComSetting(SettingContext settingContext, PtAndComSetting ptAndComSetting);

        IList<CommandResult> UpdateMasterPtAndComSetting(SettingContext settingContext, PtAndComSetting ptAndComSetting);

        IList<CommandResult> UpdateAgentPtAndComSetting(SettingContext settingContext, PtAndComSetting ptAndComSetting);

        IList<CommandResult> AgentUpdateMemberPtAndComSetting(SettingContext settingContext, PtAndComSetting ptAndComSetting);

        IList<CommandResult> SuperUpdateMemberPtAndComSetting(SettingContext settingContext, PtAndComSetting ptAndComSetting);

        IList<CommandResult> MasterUpdateMemberPtAndComSetting(SettingContext settingContext, PtAndComSetting ptAndComSetting);

        #endregion

        #region Win Limit

        CurrentMemberWin GetCurrentWinLimit(int custId);

        ICommandResult UpdateWinLimit(SettingContext settingContext, MemberWinLimit memberWinLimit);

        #endregion

        #region Game Setting

        List<GameType> GetGameSettingByExchange(int exchangeId, string site);

        List<GameType> GetGameSettingByCustomerId(int custId);

        #endregion
    }
}
