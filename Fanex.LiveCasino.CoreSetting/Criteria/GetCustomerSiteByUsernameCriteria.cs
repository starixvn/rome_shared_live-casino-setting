namespace Fanex.LiveCasino.CoreSetting.Criteria
{
    using Fanex.NAP.Framework.Data;

    public class GetCustomerSiteByUsernameCriteria : CriteriaBase
    {
        public string Username { get; set; }

        public override bool IsValid()
        {
            return !string.IsNullOrWhiteSpace(Username);
        }

        public override string GetSettingKey()
        {
            return "Acc_CustomerSelSiteByUserName";
        }
    }
}
