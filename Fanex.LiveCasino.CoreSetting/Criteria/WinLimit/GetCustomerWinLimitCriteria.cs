namespace Fanex.LiveCasino.CoreSetting.Criteria.WinLimit
{
    using System.Diagnostics.CodeAnalysis;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Setting key : Acc_CustomerSelWinLimitLiveCS.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed.")]
    internal class GetCustomerWinLimitCriteria : CustomerCriteriaBase
    {
        public override string GetSettingKey()
        {
            return "Acc_CustomerSelWinLimitLiveCS";
        }
    }
}