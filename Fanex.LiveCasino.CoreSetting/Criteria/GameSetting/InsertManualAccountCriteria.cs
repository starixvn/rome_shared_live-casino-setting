namespace Fanex.LiveCasino.CoreSetting.Criteria.GameSetting
{
    /// <summary>
    /// Setting key : TP_User_Customer_ManualInsertAccount.
    /// </summary>
    public class InsertManualAccountCriteria : InsertAutoAccountCriteria
    {
        public string CustSite { get; set; }

        public override string GetSettingKey()
        {
            return "TP_User_Customer_ManualInsertAccount";
        }
    }
}
