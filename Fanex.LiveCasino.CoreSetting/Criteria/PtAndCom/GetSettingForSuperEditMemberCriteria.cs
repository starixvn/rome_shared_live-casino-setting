namespace Fanex.LiveCasino.CoreSetting.Criteria.PtAndCom
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Get Setting For Super Edit Member Criteria.
    /// </summary>
    public class GetSettingForSuperEditMemberCriteria : CriteriaBase
    {
        public int CustId { get; set; }

        public override string GetSettingKey()
        {
            return "Acc_CustomerSelSEditCustomerLiveCS";
        }

        public override bool IsValid()
        {
            return CustId > 0;
        }
    }
}