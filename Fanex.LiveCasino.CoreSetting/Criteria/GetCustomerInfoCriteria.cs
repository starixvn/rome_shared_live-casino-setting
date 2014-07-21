// -----------------------------------------------------------------------
// <copyright file="GetCustomerInfoCriteria.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.LiveCasino.CoreSetting.Criteria
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fanex.LiveCasino.CoreSetting.Core.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class GetCustomerInfoCriteria : CriteriaBase
    {
        public int Custid { get; set; }

        public override string GetSettingKey()
        {
            return "Acc_CustomerSelInfoLiveCS";
        }

        public override bool IsValid()
        {
            return Custid > 0;
        }
    }
}
