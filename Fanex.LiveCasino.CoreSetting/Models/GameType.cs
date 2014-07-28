namespace Fanex.LiveCasino.CoreSetting.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class GameType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int BetTypeId { get; set; }
        public List<StakeLevel> BettingLimit { get; set; }
    }
}
