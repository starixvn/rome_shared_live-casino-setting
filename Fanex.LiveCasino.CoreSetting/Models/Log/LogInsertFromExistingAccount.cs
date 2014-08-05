namespace Fanex.LiveCasino.CoreSetting.Models.Log
{
    /// <summary>
    /// Log insert account from existing account.
    /// </summary>
    public class LogInsertFromExistingAccount : LogAutoInsertAccount
    {
        public override string SpName
        {
            get
            {
                return "TP_User_Customer_AutoInsertAccountFromExisting";
            }
        }
    }
}