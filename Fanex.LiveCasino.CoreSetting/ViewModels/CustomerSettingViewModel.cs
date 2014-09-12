namespace Fanex.LiveCasino.CoreSetting.ViewModels
{
    using System.Collections.Generic;
    using Fanex.LiveCasino.CoreSetting.Models;

    /// <summary>
    /// Customer Setting View Model.
    /// </summary>
    public class CustomerSettingViewModel
    {
        public LiveCasinoSettingContext SettingContext { get; set; }

        public Customer Customer { get; set; }
    }
}