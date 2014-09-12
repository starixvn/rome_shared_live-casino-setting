namespace Fanex.LiveCasino.CoreSetting.Models
{
    using System.Collections.Generic;

    /// <summary>
    /// Customer setting live casino.
    /// </summary>
    public class LiveCasinoSetting
    {
        public WinLimit WinLimit { get; set; }

        public float Commission { get; set; }

        public PositionTaking PositionTaking { get; set; }

        public List<GameType> GameSetting { get; set; }
    }
}