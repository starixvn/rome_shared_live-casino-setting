
namespace Fanex.LiveCasino.CoreSetting.Criteria.PtAndCom
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class GetSettingForEditAgentCriteria : CriteriaBase
    {
        public int CustId { get; set; }

        public int MRecommend { get; set; }

        public override string GetSettingKey()
        {
            return "Acc_CustomerSelEditAgentLiveCS";
        }

        public override bool IsValid()
        {
            return CustId > 0 && MRecommend > 0;
        }
    }
}
