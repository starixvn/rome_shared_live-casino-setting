namespace Fanex.LiveCasino.CoreSetting.ViewModels
{
    using System.Collections.Generic;
    using Fanex.LiveCasino.CoreSetting.Models;

    /// <summary>
    /// Customer Setting View Model.
    /// </summary>
    public class CustSettingViewModel
    {
        public SettingContext SettingContext { get; set; }

        public CustInfo CustInfo { get; set; }

        public PtAndComSetting PtAndComSetting { get; set; }

        public List<GameType> GameSetting { get; set; }
    }
}