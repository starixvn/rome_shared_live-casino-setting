namespace Fanex.LiveCasino.CoreSetting.Models
{
    /// <summary>
    /// Stake level.
    /// </summary>
    public class StakeLevel
    {
        public int Id { get; set; }

        public string Range { get; set; }

        public bool IsChecked { get; set; }
    }
}