// --------------------------------------------------------------------------
// <copyright file="CustSettingViewModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2014.8.4">Created</change>
// </history>

namespace Fanex.LiveCasino.CoreSetting.ViewModels
{
    using System.Collections.Generic;
    using Fanex.LiveCasino.CoreSetting.Models;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class CustSettingViewModel
    {
        public SettingContext SettingContext { get; set; }

        public CustInfo CustInfo { get; set; }

        public PtAndComSetting PtAndComSetting { get; set; }

        public IList<GameType> GameSetting { get; set; }
    }
}