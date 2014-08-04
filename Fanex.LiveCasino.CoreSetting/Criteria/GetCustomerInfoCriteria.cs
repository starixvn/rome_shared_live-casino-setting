// --------------------------------------------------------------------------
// <copyright file="GetCustomerInfoCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2014.8.4">Created</change>
// </history>
namespace Fanex.LiveCasino.CoreSetting.Criteria
{
    using Fanex.NAP.Framework.Data;

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
