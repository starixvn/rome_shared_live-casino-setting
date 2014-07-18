// -----------------------------------------------------------------------
// <copyright file="MemberWinLimit.cs" company="">
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
    public class MemberWinLimit
    {
        public decimal Max { get; set; }
        public bool IsAutoReset { get; set; }

        public decimal Current { get; set; }
        public MemberWinLimit Upline { get; set; }
    }
}
