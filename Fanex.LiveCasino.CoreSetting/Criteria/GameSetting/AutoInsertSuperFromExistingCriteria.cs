namespace Fanex.LiveCasino.CoreSetting.Criteria.GameSetting
{
    /// <summary>
    ///  Setting key : TP_User_Customer_AutoInsertAccountFromExisting.
    /// </summary>
    internal class AutoInsertSuperFromExistingCriteria : AutoInsertAccountFromExistingCriteria
    {
        public string CustSite { get; set; }

        public override string GetSettingKey()
        {
            return "TP_User_Customer_AutoInsertSuperFromExisting";
        }

        public override string ToString()
        {
            return base.ToString() + "\nCustSite = '" + CustSite + "'";
        }
    }
}
