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
    }
}