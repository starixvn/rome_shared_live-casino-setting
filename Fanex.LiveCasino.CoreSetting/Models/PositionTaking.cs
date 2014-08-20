namespace Fanex.LiveCasino.CoreSetting.Models
{
    /// <summary>
    /// Position Taking.
    /// </summary>
    public class PositionTaking : PositionTakingMatrix
    {
        public bool? IsDisableAuto { get; set; }

        public float? S1Min { get; set; }

        public float? S1Max { get; set; }

        public float? M1Max { get; set; }

        public float? M1Min { get; set; }

        public float? NewPresetOnMember { get; set; }

        public bool? UpdateAllMembers { get; set; }
    }
}