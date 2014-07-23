// -----------------------------------------------------------------------
// <copyright file="CustomerSetting.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.LiveCasino.CoreSetting.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fanex.LiveCasino.CoreSetting.Models;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class CustSettingViewModel
    {
        public SettingContext SettingContext { get; set; }

        public CustInfo CustInfo { get; set; }

        public PtAndComSettingViewModel PtAndComSetting { get; set; }

        public IList<GameType> GameSetting { get; set; }

        
    }
}
