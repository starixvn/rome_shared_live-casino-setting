namespace Fanex.LiveCasino.CoreSetting.Criteria.PtAndCom
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Get Setting For Master Edit Member Criteria.
    /// </summary>
    public class GetSettingForMasterEditMemberCriteria : CriteriaBase
    {
        public int CustId { get; set; }

        public override string GetSettingKey()
        {
            return "Acc_CustomerSelMEditCustomerLiveCS";
        }

        public override bool IsValid()
        {
            return CustId > 0;
        }
    }
}