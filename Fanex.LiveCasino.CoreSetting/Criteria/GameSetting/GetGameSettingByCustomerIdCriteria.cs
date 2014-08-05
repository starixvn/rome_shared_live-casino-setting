namespace Fanex.LiveCasino.CoreSetting.Criteria.GameSetting
{
    using System.Diagnostics.CodeAnalysis;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Setting key : TP_Admin_StakeLevel_GetCustStakeLevelMulti.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed.")]
    public class GetGameSettingByCustomerIdCriteria : CriteriaBase
    {
        public int CustId { get; set; }

        public override bool IsValid()
        {
            return CustId > 0;
        }

        public override string GetSettingKey()
        {
            return "TP_Admin_StakeLevel_GetCustStakeLevelMulti";
        }
    }
}