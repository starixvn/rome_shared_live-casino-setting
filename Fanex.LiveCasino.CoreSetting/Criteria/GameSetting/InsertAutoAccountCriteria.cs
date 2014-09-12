namespace Fanex.LiveCasino.CoreSetting.Criteria.GameSetting
{
    using System.Diagnostics.CodeAnalysis;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Setting key : TP_User_Customer_AutoInsertAccount.
    /// </summary>
    internal class InsertAutoAccountCriteria : CustomerCriteriaBase
    {
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

        [SpParam(Ignored = true)]
        public string Language { get; set; }

        public override string GetSettingKey()
        {
            return "TP_User_Customer_AutoInsertAccount";
        }

        public override string ToString()
        {
            return string.Format("SP Name = '{0}'\n{1}", GetSettingKey(), GetString());
        }

        /// <summary>
        /// Gets the string.
        /// </summary>
        /// <returns>String content.</returns>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules", "SP2100:CodeLineMustNotBeLongerThan", Justification = "Reviewed. Suppression is OK here.")]
        protected string GetString()
        {
            return string.Format(
                "CustId = '{0}'\nCasinoNo = '{1}'\nExchangeId = '{2}'\nSuperId = '{3}'\nMasterId = '{4}'\nAgentId = '{5}'\nRoleId = '{6}'\nSignupIp = '{7}'\nSignupBy = '{8}'\nSignupFrom = '{9}'\nModifyUsername = '{10}'\nUsername = '{11}'",
                CustId,
                CasinoNo,
                ExchangeId,
                SuperId,
                MasterId,
                AgentId,
                RoleId,
                SignupIp,
                SignupBy,
                SignupFrom,
                ModifyUsername,
                Username);
        }
    }
}