// --------------------------------------------------------------------------
// <copyright file="UpdateMasterSettingCriteria.cs" company="Nexcel Solutions Vietnam">
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
    public class UpdateMasterSettingCriteria : CriteriaBase
    {
        public int Custid { get; set; }

        public int SRecommend { get; set; }

        public decimal V162spositiontaking_1101 { get; set; }

        public decimal V162mpositiontaking_1101 { get; set; }

        public bool IsAuto_v162spositiontaking_1101 { get; set; }

        public decimal Auto_v162spositiontaking_1101 { get; set; }

        public decimal V162mpositiontaking_1101_min { get; set; }

        public decimal V162discount_1101 { get; set; }

        public bool V162UpdatePositiontaking_1101
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

        public bool V162UpdateAutoPositiontaking_1101
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

        public decimal NewSPositionTaking { get; set; }

        public int SubAccID { get; set; }

        public int UserId { get; set; }

        public string IP { get; set; }

        public string Language { get; set; }

        public override string GetSettingKey()
        {
            return "Acc_CustomerUpdMasterLiveCS_All";
        }

        public override bool IsValid()
        {
            return Custid > 0 && SRecommend > 0 & UserId > 0;
        }
    }
}