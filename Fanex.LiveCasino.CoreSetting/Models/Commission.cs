namespace Fanex.LiveCasino.CoreSetting.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class Commission
    {
        public decimal Value { get; set; }

        public Commission Upline { get; set; }
    }
}
