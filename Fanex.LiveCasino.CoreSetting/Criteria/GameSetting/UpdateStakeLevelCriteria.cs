namespace Fanex.LiveCasino.CoreSetting.Criteria.GameSetting
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Setting key : TP_User_CustGameType_UpdStakeLevel.
    /// </summary>
    public class UpdateStakeLevelCriteria : CriteriaBase
    {
        public int CustId { get; set; }

        public int RoleId { get; set; }

        public int GameTypeId { get; set; }

        public string StakeLevelList { get; set; }

        public override bool IsValid()
        {
            return CustId > 0;
        }

        public override string GetSettingKey()
        {
            return "TP_User_CustGameType_UpdStakeLevel";
        }
    }
}