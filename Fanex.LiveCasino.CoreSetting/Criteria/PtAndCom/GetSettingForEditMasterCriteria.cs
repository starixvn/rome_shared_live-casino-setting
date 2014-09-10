namespace Fanex.LiveCasino.CoreSetting.Criteria.PtAndCom
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Get Setting For Edit Master Criteria.
    /// </summary>
    internal class GetSettingForEditMasterCriteria : CustomerCriteriaBase
    {
        public int SRecommend { get; set; }

        public override string GetSettingKey()
        {
            return "Acc_CustomerSelEditMasterLiveCS";
        }

        public override bool IsValid()
        {
            return base.IsValid() && SRecommend > 0;
        }
    }
}