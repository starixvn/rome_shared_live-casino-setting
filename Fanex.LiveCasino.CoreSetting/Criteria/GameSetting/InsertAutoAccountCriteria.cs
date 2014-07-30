namespace Fanex.LiveCasino.CoreSetting.Criteria.GameSetting
{
    using Fanex.NAP.Framework.Data;

    public class InsertAutoAccountCriteria : CriteriaBase
    {
        public int CustId { get; set; }

        public string CasinoNo { get; set; }

        public int ExchangeId { get; set; }

        [SpParam(SqlParamName = "SCustId")]
        public int SuperId { get; set; }

        [SpParam(SqlParamName = "MCustId")]
        public int MasterId { get; set; }

        [SpParam(SqlParamName = "ACustId")]
        public int AgentId { get; set; }

        public int RoleId { get; set; }

        public string SignupIp { get; set; }

        public int SignupBy { get; set; }

        public int SignupFrom { get; set; }

        public string ModifyUsername { get; set; }

        public string Username { get; set; }

        public override bool IsValid()
        {
            return CustId > 0;
        }

        public override string GetSettingKey()
        {
            return "TP_User_Customer_AutoInsertAccount";
        }
    }
}
