// --------------------------------------------------------------------------
// <copyright file="GetSettingForEditMemberCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2014.8.4">Created</change>
// </history>
namespace Fanex.LiveCasino.CoreSetting.Criteria.PtAndCom
{
    using System;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// TODO: Update summary.
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