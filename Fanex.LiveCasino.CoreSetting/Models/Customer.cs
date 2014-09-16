namespace Fanex.LiveCasino.CoreSetting.Models
{
    using Fanex.LiveCasino.CoreSetting.Enumerations;

    /// <summary>
    /// Customer information.
    /// </summary>
    public class Customer : User
    {
        public byte CurrencyId { get; set; }

        public string Site { get; set; }

        public CustomerLevel Level { get; set; }

        public bool IsSyncLiveCasino { get; set; }

        public bool DisableLiveCasino { get; set; }

        public Customer Parent { get; set; }

        public LiveCasinoSetting LiveCasinoSetting { get; set; }

        public void SetUplineIdentifier(ref int agentId, ref int masterId, ref int superId)
        {
            switch (Level)
            {
                case CustomerLevel.Member:
                    superId = Parent.Parent.Parent.Id;
                    masterId = Parent.Parent.Id;
                    agentId = Parent.Id;
                    break;
                case CustomerLevel.Agent:
                    superId = Parent.Parent.Id;
                    masterId = Parent.Id;
                    agentId = Id;
                    break;
                case CustomerLevel.Master:
                    superId = Parent.Id;
                    masterId = Id;
                    break;
                default:
                    superId = Id;
                    break;
            }
        }
    }
}