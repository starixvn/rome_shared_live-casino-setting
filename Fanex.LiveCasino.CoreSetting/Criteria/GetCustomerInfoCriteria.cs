namespace Fanex.LiveCasino.CoreSetting.Criteria
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Get customer information criteria.
    /// </summary>
    public class GetCustomerInfoCriteria : CriteriaBase
    {
        public int Custid { get; set; }

        public override string GetSettingKey()
        {
            return "Acc_CustomerSelInfoLiveCS";
        }

        public override bool IsValid()
        {
            return Custid > 0;
        }
    }
}
