// --------------------------------------------------------------------------
// <copyright file="UpdateAgentSettingCriteria.cs" company="Nexcel Solutions Vietnam">
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
    /// TODO: Update summary.
    /// </summary>
    public class UpdateAgentSettingCriteria : CriteriaBase
    {
        public int CustId { get; set; }

        public int MRecommend { get; set; }

        public decimal V162mpositiontaking_1101 { get; set; }

        public decimal Auto_v162mpositiontaking_1101 { get; set; }

        public bool IsAuto_v162mpositiontaking_1101 { get; set; }

        public decimal V162apositiontaking_1101 { get; set; }

        public decimal Min_v162apositiontaking_1101 { get; set; }

        public decimal V162discount_1101 { get; set; }

        public bool V162UpdatePositiontaking_1101
        {
            get
            {
                return true;
            }
        }

        public bool V162UpdateAutoPositiontaking_1101
        {
            get
            {
                return true;
            }
        }

        public bool V162UpdateMinPositiontaking_1101
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

        public bool UpdateAllMembers { get; set; }

        public decimal NewMPositionTaking { get; set; }

        public int UserId { get; set; }

        public int SubAccID { get; set; }

        public string Language { get; set; }

        public string IP { get; set; }

        public override string GetSettingKey()
        {
            return "Acc_CustomerUpdAgentLiveCS_All";
        }

        public override bool IsValid()
        {
            return CustId > 0 && MRecommend > 0 && UserId > 0;
        }
    }
}