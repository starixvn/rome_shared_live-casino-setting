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
    public class PositionTaking
    {
        public decimal Min { get; set; }
        public decimal Max { get; set; }
        public decimal Preset { get; set; }

        public bool IsAuto { get; set; }
        public decimal AutoPreset { get; set; }

        public PositionTaking Upline { get; set; }
    }
}
