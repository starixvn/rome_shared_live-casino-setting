namespace Fanex.LiveCasino.CoreSetting.Models.Log
{
    using Fanex.LiveCasino.CoreSetting.Enumerations;

    /// <summary>
    /// Log manual insert account to Live Casino setting.
    /// </summary>
    internal class ManualInsertAccountLog : GameSettingLog, IGameSettingLog
    {
        private const string TrackingTemplate = "<{0} CustID=\"{1}\" UserName=\"{2}\" CasinoNo=\"{3}\" ExchangeID=\"{4}\"" +
                                                " SCustID=\"{5}\" MCustID=\"{6}\" ACustID=\"{7}\" RoleID=\"{8}\" SignupIP=\"{9}\" SignupBy=\"{10}\"" +
                                                " AddFrom=\"{11}\" Lang=\"{12}\" ErrCode=\"{13}\" ErrMsg=\"{14}\" {15} CustSite=\"{16}\" />";

        public string CasinoNo { get; set; }

        public override LogActions Action
        {
            get
            {
                LogActions logAction;

                switch (SettingContext.Customer.Level)
                {
                    case CustomerLevel.Super:
                        logAction = LogActions.ManualInsert_Super;
                        break;

                    case CustomerLevel.Master:
                        logAction = LogActions.ManualInsert_Master;
                        break;

                    case CustomerLevel.Agent:
                        logAction = LogActions.ManualInsert_Agent;
                        break;

                    default:
                        logAction = LogActions.ManualInsert_Member;
                        break;
                }

                return logAction;
            }
        }

        public override string NewValue
        {
            get
            {
                int agentId = 0, masterId = 0, superId = 0;
                SettingContext.Customer.SetUplineIdentifier(ref agentId, ref masterId, ref superId);

                return string.Format(
                    TrackingTemplate,
                    SpName,
                    SettingContext.Customer.Id,
                    SettingContext.Customer.Username,
                    CasinoNo,
                    SettingContext.Customer.CurrencyId,
                    superId,
                    masterId,
                    agentId,
                    (int)SettingContext.Customer.Level,
                    SettingContext.IP,
                    SettingContext.Actor.Id,
                    SignUpForm,
                    SettingContext.Language,
                    ErrCode,
                    ErrMsg,
                    TrackingSubAccount,
                    SettingContext.Customer.Site);
            }
        }

        public override string OldValue
        {
            get
            {
                return string.Empty;
            }
        }

        public int SignUpForm { get; set; }

        public override string SpName
        {
            get
            {
                return "TP_User_Customer_ManualInsertAccount";
            }
        }
    }
}