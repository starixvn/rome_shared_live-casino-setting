namespace Fanex.LiveCasino.CoreSetting.Criteria
{
    using System.Diagnostics.CodeAnalysis;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Setting key : Acc_CustomerSelSiteByUserName.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed.")]
    public class GetCustomerSiteByUsernameCriteria : CriteriaBase
    {
        public string Username { get; set; }

        public override bool IsValid()
        {
            return !string.IsNullOrWhiteSpace(Username);
        }

        public override string GetSettingKey()
        {
            return "Acc_CustomerSelSiteByUserName";
        }
    }
}