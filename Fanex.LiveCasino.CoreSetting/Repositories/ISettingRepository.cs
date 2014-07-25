namespace Fanex.LiveCasino.CoreSetting.Repositories
{
    using Fanex.LiveCasino.CoreSetting.Models;
    using Fanex.NAP.Framework.Data;
    using Fanex.LiveCasino.CoreSetting.ViewModels;
    using Fanex.LiveCasino.CoreSetting.Criteria.PtAndCom;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface ISettingRepository : IBaseRepository
    {
        CustInfo GetCustomerInfo(ICriteria criteria);

        #region Get Setting

        PtAndComSetting GetSettingForEditSuper(GetSettingForEditSuperCriteria criteria);

        PtAndComSetting GetSettingForEditMaster(GetSettingForEditMasterCriteria criteria);

        PtAndComSetting GetSettingForEditAgent(GetSettingForEditAgentCriteria criteria);

        PtAndComSetting GetSettingForEditMember(GetSettingForEditMemberCriteria criteria);

        PtAndComSetting GetSettingForSuperEditMember(GetSettingForSuperEditMemberCriteria criteria);

        PtAndComSetting GetSettingForMasterEditMember(GetSettingForMasterEditMemberCriteria criteria);
        
        #endregion
    }
}
