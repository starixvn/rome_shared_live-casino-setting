namespace Fanex.LiveCasino.CoreSetting.DTO
{
    using Fanex.LiveCasino.CoreSetting.Models;

    /// <summary>
    /// PT Commmission DTO for getting data.
    /// </summary>
    public class PTComDTO
    {
        public float Commission { get; set; }

        public float ParentCommission { get; set; }

        public PositionTaking PositionTaking { get; set; }
    }
}