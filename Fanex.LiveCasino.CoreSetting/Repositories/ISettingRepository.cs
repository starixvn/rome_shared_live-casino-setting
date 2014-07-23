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

        PtAndComSettingViewModel GetSettingForEditSuper(GetSettingForEditSuperCriteria criteria);

        PtAndComSettingViewModel GetSettingForEditMaster(GetSettingForEditMasterCriteria criteria);

        PtAndComSettingViewModel GetSettingForEditAgent(GetSettingForEditAgentCriteria criteria);

        PtAndComSettingViewModel GetSettingForEditMember(GetSettingForEditMemberCriteria criteria);

        PtAndComSettingViewModel GetSettingForSuperEditMember(GetSettingForSuperEditMemberCriteria criteria);

        PtAndComSettingViewModel GetSettingForMasterEditMember(GetSettingForMasterEditMemberCriteria criteria);
        
        #endregion
    }
}
