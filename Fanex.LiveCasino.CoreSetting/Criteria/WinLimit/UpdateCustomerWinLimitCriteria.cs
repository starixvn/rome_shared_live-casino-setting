namespace Fanex.LiveCasino.CoreSetting.Criteria.WinLimit
{
    using System.Diagnostics.CodeAnalysis;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Setting key : Acc_CustomerUpdLiveCS_WinLimit.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed.")]
    public class UpdateCustomerWinLimitCriteria : CriteriaBase
    {
        public int CustId { get; set; }

        [SpParam(SqlParamName = "MaxWinLiveCasino")]
        public decimal MaxWinLimit { get; set; }

        [SpParam(SqlParamName = "IsAutoResetLiveCasino")]
        public bool IsAutoDailyReset { get; set; }

        public int SubAccID { get; set; }

        public string Ip { get; set; }

        public int UserId { get; set; }

        public string Language { get; set; }

        public override string GetSettingKey()
        {
            return "Acc_CustomerUpdLiveCS_WinLimit";
        }

        public override bool IsValid()
        {
            return CustId > 0;
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format(
                "SP Name = '{0}'\nCustId = '{1}'\nMaxWinLimit = '{2}'\nIsAutoDailyReset = '{3}'\nUserId = '{4}'\nSubAccID= '{5}'\nIp = '{6}'\nLanguage = '{7}'",
                GetSettingKey(),
                CustId,
                MaxWinLimit,
                IsAutoDailyReset,
                UserId,
                SubAccID,
                Ip,
                Language);
        }
    }
}