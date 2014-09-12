namespace Fanex.LiveCasino.CoreSetting.Models.Log
{
    using Fanex.LiveCasino.CoreSetting.Enumerations;

    /// <summary>
    /// Log insert account from existing account.
    /// </summary>
    internal class InsertFromExistingAccountLog : AutoInsertAccountLog
    {
        public override LogActions Action
        {
            get
            {
                LogActions logAction;

                switch (SettingContext.CustomerLevel)
                {
                    case CustomerLevel.Master:
                        logAction = LogActions.AutoInsertExisting_Master;
                        break;
                    case CustomerLevel.Agent:
                        logAction = LogActions.AutoInsertExisting_Agent;
                        break;
                    default:
                        logAction = LogActions.AutoInsertExisting_Member;
                        break;
                }

                return logAction;
            }
        }

        public override string SpName
        {
            get
            {
                return "TP_User_Customer_AutoInsertAccountFromExisting";
            }
        }
    }
}