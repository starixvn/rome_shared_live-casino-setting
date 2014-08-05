namespace Fanex.LiveCasino.CoreSetting.Criteria.PtAndCom
{
    using System;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Get Setting For Edit Member Criteria.
    /// </summary>
    public class GetSettingForEditMemberCriteria : CriteriaBase
    {
        public int CustId { get; set; }

        public int Recommend { get; set; }

        public override string GetSettingKey()
        {
            return "Acc_CustomerSelEditMemberLiveCS";
        }

        public override bool IsValid()
        {
            return CustId > 0 && Recommend > 0;
        }
    }
}