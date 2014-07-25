namespace Fanex.LiveCasino.CoreSetting.Criteria.WinLimit
{
    using Fanex.NAP.Framework.Data;

    public class GetCustomerWinLimitCriteria : CriteriaBase
    {
        public int CustId { get; set; }

        public override string GetSettingKey()
        {
            return "Acc_CustomerSelWinLimitLiveCS";
        }

        public override bool IsValid()
        {
            return CustId >= 0;
        }
    }
}
