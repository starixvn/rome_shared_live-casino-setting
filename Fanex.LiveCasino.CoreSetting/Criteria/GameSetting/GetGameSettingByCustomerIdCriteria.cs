namespace Fanex.LiveCasino.CoreSetting.Criteria.GameSetting
{
    using Fanex.NAP.Framework.Data;

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
