namespace Fanex.LiveCasino.CoreSetting.Models
{
    /// <summary>
    /// Commission model.
    /// </summary>
    public class Commission
    {
        public float Value { get; set; }

        public Commission Upline { get; set; }
    }
}