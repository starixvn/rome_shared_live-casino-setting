namespace Fanex.LiveCasino.CoreSetting.DTO
{
    /// <summary>
    /// Up line customer information DTO.
    /// </summary>
    public class CustomerInfoDTO
    {
        public int CustId { get; set; }

        public string Username { get; set; }

        public int AgentId { get; set; }

        public int MasterId { get; set; }

        public int SuperId { get; set; }

        public int CurrencyId { get; set; }

        public byte RoleId { get; set; }
    }
}