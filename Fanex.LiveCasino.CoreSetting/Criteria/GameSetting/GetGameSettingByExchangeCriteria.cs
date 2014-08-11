namespace Fanex.LiveCasino.CoreSetting.Criteria.GameSetting
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Setting key : TP_Admin_StakeLevel_GetGameTypeStakeLevel.
    /// </summary>
    public class GetGameSettingByExchangeCriteria : CriteriaBase
    {
        public int ExchangeId { get; set; }

        public string Site { get; set; }

        public override bool IsValid()
        {
            return ExchangeId > 0;
        }

        public override string GetSettingKey()
        {
            return "TP_Admin_StakeLevel_GetGameTypeStakeLevel";
        }
    }
}