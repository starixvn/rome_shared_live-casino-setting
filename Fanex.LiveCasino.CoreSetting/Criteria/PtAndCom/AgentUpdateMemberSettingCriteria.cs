// --------------------------------------------------------------------------
// <copyright file="AgentUpdateMemberSettingCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2014.8.4">Created</change>
// </history>

namespace Fanex.LiveCasino.CoreSetting.Criteria.PtAndCom
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Agent Update Member Setting Criteria.
    /// </summary>
    public class AgentUpdateMemberSettingCriteria : CriteriaBase
    {
        public int CustId { get; set; }

        public int Recommend { get; set; }

        public decimal V162apositiontaking_1101 { get; set; }

        public decimal V162playerdiscount_1101 { get; set; }

        public bool V162UpdatePositiontaking_1101
        {
            get
            {
                return true;
            }
        }

        public bool V162UpdateCommission_1101
        {
            get
            {
                return true;
            }
        }

        public int SubAccID { get; set; }

        public string IP { get; set; }

        public int UserId { get; set; }

        public string Language { get; set; }

        public override string GetSettingKey()
        {
            return "Acc_CustomerUpdMemberLiveCS_All";
        }

        public override bool IsValid()
        {
            return CustId > 0 && Recommend > 0 && UserId > 0;
        }
    }
}