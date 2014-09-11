namespace Fanex.LiveCasino.CoreSetting.DTO
{
    using Fanex.LiveCasino.CoreSetting.Models;

    /// <summary>
    /// Position Taking and Commission for get data from database.
    /// </summary>
    public class PTComSettingDTO
    {
        public PTComSettingDTO()
        {
            Commission = new Commission();

            PositionTaking = new PositionTaking();
        }

        public Commission Commission { get; set; }

        public PositionTaking PositionTaking { get; set; }
    }
}