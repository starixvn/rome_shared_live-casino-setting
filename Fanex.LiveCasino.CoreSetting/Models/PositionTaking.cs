// --------------------------------------------------------------------------
// <copyright file="PositionTaking.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2014.8.4">Created</change>
// </history>
namespace Fanex.LiveCasino.CoreSetting.Models
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class PositionTaking : PositionTakingMatrix
    {
        public bool? IsDisableAuto { get; set; }

        public decimal? S1Min { get; set; }

        public decimal? S1Max { get; set; }

        public decimal? M1Max { get; set; }

        public decimal? M1Min { get; set; }

        public decimal? Us3Max { get; set; }

        public decimal? Us1Max { get; set; }

        public decimal? Um2Max { get; set; }

        public decimal? Um1Max { get; set; }

        public decimal? NewPresetOnMember { get; set; }

        public bool? UpdateAllMembers { get; set; }
    }
}