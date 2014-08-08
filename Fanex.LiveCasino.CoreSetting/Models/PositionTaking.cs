namespace Fanex.LiveCasino.CoreSetting.Models
{
    /// <summary>
    /// Position Taking.
    /// </summary>
    public class PositionTaking : PositionTakingMatrix
    {
        public bool? IsDisableAuto { get; set; }

        public decimal? S1Min { get; set; }

        public decimal? S1Max { get; set; }

        public decimal? M1Max { get; set; }

        public decimal? M1Min { get; set; }

        public decimal? NewPresetOnMember { get; set; }

        public bool? UpdateAllMembers { get; set; }
    }
}