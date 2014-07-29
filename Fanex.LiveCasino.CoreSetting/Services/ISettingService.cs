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

        ICommandResult UpdateSyncLiveCasino(SettingContext settingContext, bool isSync);

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

        IList<ICommandResult> UpdateSuperPtAndComSetting(SettingContext settingContext, PtAndComSetting ptAndComSetting);

        IList<ICommandResult> UpdateMasterPtAndComSetting(SettingContext settingContext, PtAndComSetting ptAndComSetting);

        IList<ICommandResult> UpdateAgentPtAndComSetting(SettingContext settingContext, PtAndComSetting ptAndComSetting);

        IList<ICommandResult> AgentUpdateMemberPtAndComSetting(SettingContext settingContext, PtAndComSetting ptAndComSetting);

        IList<ICommandResult> SuperUpdateMemberPtAndComSetting(SettingContext settingContext, PtAndComSetting ptAndComSetting);

        IList<ICommandResult> MasterUpdateMemberPtAndComSetting(SettingContext settingContext, PtAndComSetting ptAndComSetting);

        IList<ICommandResult> UpdatePtAndComSetting(SettingContext settingContext, PtAndComSetting ptAndComSetting);

        #endregion

        #region Win Limit

        CurrentMemberWin GetCurrentWinLimit(int custId);

        ICommandResult UpdateWinLimit(SettingContext settingContext, MemberWinLimit memberWinLimit);

        #endregion

        #region Game Setting

        List<GameType> GetGameSettingByExchange(int exchangeId, string site);

        List<GameType> GetGameSettingByCustomerId(int custId);

        ICommandResult InsertAutoAccount(SettingContext settingContext);

        ICommandResult InsertManualAccount(SettingContext settingContext);

        ICommandResult UpdateStakeLevel(int custId, int roleId, GameType gameType);

        List<ICommandResult> UpdateListStakeLevel(int custId, int roleId, List<GameType> gameTypes);

        List<ICommandResult> UpdateManualGameSetting(SettingContext settingContext, CustInfo custInfo);

        #endregion
    }
}
