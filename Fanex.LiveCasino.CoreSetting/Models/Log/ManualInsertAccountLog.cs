namespace Fanex.LiveCasino.CoreSetting.Models.Log
{
    using Fanex.LiveCasino.CoreSetting.Enumerations;
    using Fanex.NAP.Framework.Data;
    using Fanex.LiveCasino.CoreSetting.Criteria.GameSetting;

    /// <summary>
    /// Log manual insert account to Live Casino setting.
    /// </summary>
    internal class ManualInsertAccountLog : GameSettingLog, IGameSettingLog
    {
        private const string TrackingTemplate = "<{0} CustID=\"{1}\" UserName=\"{2}\" CasinoNo=\"{3}\" ExchangeID=\"{4}\"" +
                                                " SCustID=\"{5}\" MCustID=\"{6}\" ACustID=\"{7}\" RoleID=\"{8}\" SignupIP=\"{9}\" SignupBy=\"{10}\"" +
                                                " AddFrom=\"{11}\" Lang=\"{12}\" ErrCode=\"{13}\" ErrMsg=\"{14}\" {15} CustSite=\"{16}\" />";

        private InsertManualAccountCriteria InsertManualAccountCriteria
        {
            get
            {
                return (InsertManualAccountCriteria)Criteria;
            }
        }

        public override ICriteria Criteria { get; set; }


        public override LogActions Action
        {
            get
            {
                LogActions logAction;

                switch (InsertManualAccountCriteria.RoleId)
                {
                    case (int)CustomerLevel.Super:
                        logAction = LogActions.ManualInsert_Super;
                        break;

                    case (int)CustomerLevel.Master:
                        logAction = LogActions.ManualInsert_Master;
                        break;

                    case (int)CustomerLevel.Agent:
                        logAction = LogActions.ManualInsert_Agent;
                        break;

                    default:
                        logAction = LogActions.ManualInsert_Member;
                        break;
                }

                return logAction;
            }
        }

        public string CasinoNo { get; set; }

        public override string NewValue
        {
            get
            {
                return string.Format(
                   TrackingTemplate,
                   SpName,
                   InsertManualAccountCriteria.CustId,
                   InsertManualAccountCriteria.Username,
                   CasinoNo,
                   InsertManualAccountCriteria.ExchangeId,
                   InsertManualAccountCriteria.SuperId,
                   InsertManualAccountCriteria.MasterId,
                   InsertManualAccountCriteria.AgentId,
                   InsertManualAccountCriteria.CustId,
                   InsertManualAccountCriteria.SignupIp,
                   InsertManualAccountCriteria.SignupBy,
                   SignUpForm,
                   InsertManualAccountCriteria.Language,
                   ErrCode,
                   ErrMsg,
                   GetTrackingSubAccount(InsertManualAccountCriteria.RoleId,InsertManualAccountCriteria.,
                   InsertManualAccountCriteria.CustSite);
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