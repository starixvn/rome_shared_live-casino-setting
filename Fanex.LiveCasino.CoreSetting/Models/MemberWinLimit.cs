namespace Fanex.LiveCasino.CoreSetting.Models
{
    /// <summary>
    /// Member win limit.
    /// </summary>
    public class MemberWinLimit
    {
        public float Max { get; set; }

        public bool IsAutoReset { get; set; }

        public float Current { get; set; }

        public MemberWinLimit Upline { get; set; }
    }
}