namespace Fanex.LiveCasino.CoreSetting.Criteria.PtAndCom
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Get Setting For Edit Super Criteria.
    /// </summary>
    public class GetSettingForEditSuperCriteria : CriteriaBase
    {
        public int Custid { get; set; }

        public override string GetSettingKey()
        {
            return "Acc_CustomerSelEditSuperLiveCS";
        }

        public override bool IsValid()
        {
            return Custid > 0;
        }
    }
}