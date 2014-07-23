namespace Fanex.LiveCasino.CoreSetting.Services
{
    using Fanex.LiveCasino.CoreSetting.Criteria;
    using Fanex.LiveCasino.CoreSetting.Models;
    using Fanex.NAP.Framework.Command;
    using Fanex.LiveCasino.CoreSetting.Criteria.PtAndCom;
    using Fanex.LiveCasino.CoreSetting.ViewModels;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface ISettingService
    {
        ICommandResult UpdateLiveCasinoStatus(UpdateLiveCasinoStatusCriteria criteria);

        string GetSettingJavascriptObjectForUpdate(SettingContext settingContext, string objectName);

        CustInfo GetCustomerInfo(GetCustomerInfoCriteria criteria);

        #region PT and Com

        PtAndComSettingViewModel GetSettingForEditSuper(GetSettingForEditSuperCriteria criteria);

        PtAndComSettingViewModel GetSettingForEditMaster(GetSettingForEditMasterCriteria criteria);

        PtAndComSettingViewModel GetSettingForEditAgent(GetSettingForEditAgentCriteria criteria);

        PtAndComSettingViewModel GetSettingForEditMember(GetSettingForEditMemberCriteria criteria);

        PtAndComSettingViewModel GetSettingForSuperEditMember(GetSettingForSuperEditMemberCriteria criteria);

        PtAndComSettingViewModel GetSettingForMasterEditMember(GetSettingForMasterEditMemberCriteria criteria);
        
        #endregion
    }
}
