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

        CustSettingViewModel GetSettingForEditSuper(GetSettingForEditSuperCriteria criteria);

        CustSettingViewModel GetSettingForEditMaster(GetSettingForEditMasterCriteria criteria);

        CustSettingViewModel GetSettingForEditAgent(GetSettingForEditAgentCriteria criteria);

        CustSettingViewModel GetSettingForEditMember(GetSettingForEditMemberCriteria criteria);

        CustSettingViewModel GetSettingForSuperEditMember(GetSettingForSuperEditMemberCriteria criteria);

        CustSettingViewModel GetSettingForMasterEditMember(GetSettingForMasterEditMemberCriteria criteria);
        
        #endregion
    }
}
