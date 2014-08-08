namespace Fanex.LiveCasino.CoreSetting.Criteria.WinLimit
{
    using System.Diagnostics.CodeAnalysis;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Setting key : Acc_CustomerSelWinLimitLiveCS.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed.")]
    public class GetCustomerWinLimitCriteria : CriteriaBase
    {
        public int CustId { get; set; }

        public override string GetSettingKey()
        {
            return "Acc_CustomerSelWinLimitLiveCS";
        }

        public override bool IsValid()
        {
            return CustId > 0;
        }
    }
}