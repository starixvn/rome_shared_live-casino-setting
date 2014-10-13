namespace Fanex.LiveCasino.CoreSetting.Models.Log
{
    using Fanex.LiveCasino.CoreSetting.Enumerations;

    /// <summary>
    /// Log insert account from existing account.
    /// </summary>
    internal class InsertFromExistingAccountLog : AutoInsertAccountLog
    {
        private const string TrackingTemplate = "<{0} CustID=\"{1}\" UserName=\"{2}\" CasinoNo=\"{3}\" ExchangeID=\"{4}\"" +
                                                " RoleID=\"{5}\" SignupIP=\"{6}\" SignupBy=\"{7}\" AddFrom=\"{8}\" Lang=\"{9}\"" +
                                                " ErrCode=\"{10}\" ErrMsg=\"{11}\" {12} CopiedFromUsername=\"{13}\">{14}</{0}>";

        public override LogActions Action
        {
            get
            {
                LogActions logAction;

                switch (SettingContext.Customer.Level)
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

        protected override string GetTrackingValue()
        {
            string gameTypeLog = GetGameTypeLogData();

            string trackingValue = string.Format(
                                           TrackingTemplate,
                                           SpName,
                                           SettingContext.Customer.Id,
                                           SettingContext.Customer.Username,
                                           CasinoNo,
                                           SettingContext.Customer.CurrencyId,
                                           (int)SettingContext.Customer.Level,
                                           SettingContext.IP,
                                           SettingContext.Actor.Id,
                                           SignUpForm,
                                           SettingContext.Language,
                                           ErrCode,
                                           ErrMsg,
                                           TrackingSubAccount,
                                           SettingContext.Customer.Username,
                                           gameTypeLog);

            return trackingValue;
        }
    }
}