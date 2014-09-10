namespace Fanex.LiveCasino.CoreSetting.DTO
{
    /// <summary>
    /// Current member win limit.
    /// </summary>
    public class CurrentMemberWinDTO
    {
        public float Balance { get; set; }

        public float MaxWinCredit { get; set; }

        public float WinPercent { get; set; }

        public float MaxWinLiveCasino { get; set; }
    }
}