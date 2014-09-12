namespace Fanex.LiveCasino.CoreSetting.Models
{
    /// <summary>
    /// Member win limit.
    /// </summary>
    public class WinLimit
    {
        public float Max { get; set; }

        public bool IsAutoReset { get; set; }

        public float Current { get; set; }
    }
}