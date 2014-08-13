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

        public int AgentId { get; set; }

        public string CasinoNo { get; set; }

        public string CustSite { get; set; }        

        public int ExchangeId { get; set; }

        public int MasterId { get; set; }

        public override string NewValue
        {
            get
            {
                return string.Format(
                    TrackingTemplate,
                   SpName,
                   CustInfo.CustId,
                   CustInfo.UserName,
                   CasinoNo,
                   ExchangeId,
                   SuperId,
                   MasterId,
                   AgentId,
                   CustInfo.RoleId,
                   SignUpIp,
                   Modifier.CustId,
                   SignUpForm,
                   Language,
                   ErrCode,
                   ErrMsg,
                   TrackingTemplate,
                   CustSite);
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

        public int SuperId { get; set; }        
    }
}