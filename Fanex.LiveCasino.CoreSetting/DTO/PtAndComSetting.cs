namespace Fanex.LiveCasino.CoreSetting.DTO
{
    using Fanex.LiveCasino.CoreSetting.Models;

    /// <summary>
    /// Position taking and Commission setting.
    /// </summary>
    public class PtAndComSetting
    {
        public PtAndComSetting()
        {
            Commission = new Commission();
            PositionTaking = new PositionTaking();
        }

        public Commission Commission { get; set; }

        public PositionTaking PositionTaking { get; set; }
    }
}