namespace Fanex.LiveCasino.CoreSetting.Models
{
    /// <summary>
    /// Member win limit.
    /// </summary>
    public class MemberWinLimit
    {
        public decimal Max { get; set; }

        public bool IsAutoReset { get; set; }

        public decimal Current { get; set; }

        public MemberWinLimit Upline { get; set; }
    }
}