namespace Fanex.LiveCasino.CoreSetting.Criteria.GameSetting
{
    using System.Diagnostics.CodeAnalysis;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Setting key : TP_User_CustGameType_UpdStakeLevel.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed.")]
    internal class UpdateStakeLevelCriteria : CustomerCriteriaBase
    {
        public int RoleId { get; set; }

        public int GameTypeId { get; set; }

        public string StakeLevelList { get; set; }

        public override bool IsValid()
        {
            return CustId > 0 && RoleId > 0 && GameTypeId > 0;
        }

        public override string GetSettingKey()
        {
            return "TP_User_CustGameType_UpdStakeLevel";
        }

        public override string ToString()
        {
            return string.Format(
                "SP Name = '{0}'\nCustId = '{1}'\nRoleId = '{2}'\nGameTypeId = '{3}'\nStakeLevelList = '{4}'",
                GetSettingKey(),
                CustId,
                RoleId,
                GameTypeId,
                StakeLevelList);
        }
    }
}