namespace Fanex.LiveCasino.CoreSetting.Criteria
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Get customer information criteria.
    /// </summary>
    internal class GetCustomerLiveCasinoInfoCriteria : CustomerCriteriaBase
    {
        public override string GetSettingKey()
        {
            return "Acc_CustomerSelInfoLiveCS";
        }
    }
}