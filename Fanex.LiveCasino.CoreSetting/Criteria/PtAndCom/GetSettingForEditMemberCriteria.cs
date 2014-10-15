namespace Fanex.LiveCasino.CoreSetting.Criteria.PtAndCom
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Get Setting For Edit Member Criteria.
    /// </summary>
    internal class GetSettingForEditMemberCriteria : CustomerCriteriaBase
    {
        public int Recommend { get; set; }

        public override string GetSettingKey()
        {
            return "Acc_CustomerSelEditMemberLiveCS";
        }

        public override bool IsValid()
        {
            return base.IsValid() && Recommend > 0;
        }
    }
}