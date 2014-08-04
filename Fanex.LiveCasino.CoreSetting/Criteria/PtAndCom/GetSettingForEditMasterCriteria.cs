// --------------------------------------------------------------------------
// <copyright file="GetSettingForEditMasterCriteria.cs" company="Nexcel Solutions Vietnam">
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
    public class GetSettingForEditMasterCriteria : CriteriaBase
    {
        public int CustId { get; set; }

        public int SRecommend { get; set; }

        public override string GetSettingKey()
        {
            return "Acc_CustomerSelEditMasterLiveCS";
        }

        public override bool IsValid()
        {
            return CustId > 0 && SRecommend > 0;
        }
    }
}