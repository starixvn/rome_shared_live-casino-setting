namespace Fanex.LiveCasino.CoreSetting.DTO
{
    using Fanex.LiveCasino.CoreSetting.Enumerations;

    /// <summary>
    /// Up line customer information DTO.
    /// </summary>
    public class UplineCustomerInfoDTO
    {
        public string Username { get; set; }

        public int CurrencyId { get; set; }

        public int RoleId { get; set; }

        public int CustId { get; set; }

        public int AgentId { get; set; }

        public int MasterId { get; set; }

        public int SuperId { get; set; }

        public int DirectUplineId
        {
            get
            {
                var directUplineId = 0;

                switch (RoleId)
                {
                    case (int)CustomerLevel.Master:
                        directUplineId = SuperId;
                        break;

                    case (int)CustomerLevel.Agent:
                        directUplineId = MasterId;
                        break;

                    default:
                        directUplineId = AgentId;
                        break;
                }

                return directUplineId;
            }
        }
    }
}