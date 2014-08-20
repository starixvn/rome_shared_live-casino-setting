namespace Fanex.LiveCasino.CoreSetting.Models
{
    /// <summary>
    /// Current member win limit.
    /// </summary>
    public class CurrentMemberWin
    {
        public float Balance { get; set; }

        public float MaxWinCredit { get; set; }

        public float WinPercent { get; set; }

        public float MaxWinLiveCasino { get; set; }
    }
}