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
    }
}