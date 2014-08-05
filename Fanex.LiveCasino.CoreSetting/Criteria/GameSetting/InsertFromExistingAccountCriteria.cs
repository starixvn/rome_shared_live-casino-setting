namespace Fanex.LiveCasino.CoreSetting.Criteria.GameSetting
{
    /// <summary>
    /// Setting key : TP_User_Customer_AutoInsertAccountFromExisting.
    /// </summary>
    public class InsertFromExistingAccountCriteria : InsertAutoAccountCriteria
    {
        public int OldCustId { get; set; }

        public string ToUserName { get; set; }

        public override string GetSettingKey()
        {
            return "TP_User_Customer_AutoInsertAccountFromExisting";
        }
    }
}