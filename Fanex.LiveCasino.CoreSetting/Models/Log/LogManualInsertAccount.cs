namespace Fanex.LiveCasino.CoreSetting.Models.Log
{
    /// <summary>
    /// Log manual insert account to Live Casino setting.
    /// </summary>
    public class LogManualInsertAccount : LogDataBase, ILogData
    {
        private const string TrackingTemplate = "<{0} CustID=\"{1}\" UserName=\"{2}\" CasinoNo=\"{3}\" ExchangeID=\"{4}\"" +
                                                " SCustID=\"{5}\" MCustID=\"{6}\" ACustID=\"{7}\" RoleID=\"{8}\" SignupIP=\"{9}\" SignupBy=\"{10}\"" +
                                                " AddFrom=\"{11}\" Lang=\"{12}\" ErrCode=\"{13}\" ErrMsg=\"{14}\" {15} CustSite=\"{16}\" />";

        public string CasinoNo { get; set; }

        public override string NewValue
        {
            get
            {
                return string.Format(
                    TrackingTemplate,
                   SpName,
                   SettingContext.TargetCustId,
                   SettingContext.TargetUsername,
                   CasinoNo,
                   SettingContext.ExchangeId,
                   SettingContext.SuperId,
                   SettingContext.MasterId,
                   SettingContext.AgentId,
                   SettingContext.TargetCustId,
                   SettingContext.IP,
                   SettingContext.UserIdDoAction,
                   SignUpForm,
                   SettingContext.Language,
                   ErrCode,
                   ErrMsg,
                   TrackingTemplate,
                   SettingContext.SiteName);
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