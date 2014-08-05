namespace Fanex.LiveCasino.CoreSetting.Models
{
    /// <summary>
    /// Current member win limit.
    /// </summary>
    public class CurrentMemberWin
    {
        public decimal Balance { get; set; }

        public decimal MaxWinCredit { get; set; }

        public decimal WinPercent { get; set; }

        public decimal MaxWinLiveCasino { get; set; }
    }
}