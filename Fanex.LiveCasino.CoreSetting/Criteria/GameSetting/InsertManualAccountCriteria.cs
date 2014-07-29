namespace Fanex.LiveCasino.CoreSetting.Criteria.GameSetting
{
    public class InsertManualAccountCriteria : InsertAutoAccountCriteria
    {
        public string CustSite { get; set; }

        public override string GetSettingKey()
        {
            return "TP_User_Customer_ManualInsertAccount";
        }
    }
}
