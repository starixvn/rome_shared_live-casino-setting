namespace Fanex.LiveCasino.CoreSetting.Models
{
    /// <summary>
    /// Position taking and Commission setting.
    /// </summary>
    public class PtAndComSetting
    {
        public Commission Commission { get; set; }

        public PositionTaking PositionTaking { get; set; }
    }
}