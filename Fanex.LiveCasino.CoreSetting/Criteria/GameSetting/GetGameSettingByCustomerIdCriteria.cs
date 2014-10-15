namespace Fanex.LiveCasino.CoreSetting.Criteria.GameSetting
{
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// Setting key : TP_Admin_StakeLevel_GetCustStakeLevelMulti.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed.")]
    internal class GetGameSettingByCustomerIdCriteria : CustomerCriteriaBase
    {
        public override string GetSettingKey()
        {
            return "TP_Admin_StakeLevel_GetCustStakeLevelMulti";
        }
    }
}