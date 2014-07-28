// -----------------------------------------------------------------------
// <copyright file="PtAndComSetting.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.LiveCasino.CoreSetting.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

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
