namespace Fanex.LiveCasino.CoreSetting.Criteria.PtAndCom
{
    /// <summary>
    /// Get setting for editing agent criteria.
    /// </summary>
    public class GetSettingForEditAgentCriteria : CustomerCriteriaBase
    {
        public int MRecommend { get; set; }

        public override string GetSettingKey()
        {
            return "Acc_CustomerSelEditAgentLiveCS";
        }

        public override bool IsValid()
        {
            return base.IsValid() && MRecommend > 0;
        }
    }
}