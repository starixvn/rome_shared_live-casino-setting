namespace Fanex.LiveCasino.CoreSetting.Models
{
    using System.Collections.Generic;

    /// <summary>
    /// Setting Context Multiple class.
    /// </summary>
    public class LiveCasinoSettingContextMultiple : LiveCasinoSettingContext
    {
        public List<Customer> Customers { get; set; }
    }
}