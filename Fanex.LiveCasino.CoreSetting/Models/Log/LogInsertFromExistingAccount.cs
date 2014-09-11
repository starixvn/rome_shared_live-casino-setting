using Fanex.LiveCasino.CoreSetting.Enumerations;
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

        public override LogActions Action
        {
            get
            {
                LogActions logAction;

                switch (SettingContext.TargetRole)
                {
                    case CustomerRole.Master:
                        logAction = LogActions.AutoInsertExisting_Master;
                        break;

                    case CustomerRole.Agent:
                        logAction = LogActions.AutoInsertExisting_Agent;
                        break;

                    default:
                        logAction = LogActions.AutoInsertExisting_Member;
                        break;
                }

                return logAction;
            }
        }
    }
}