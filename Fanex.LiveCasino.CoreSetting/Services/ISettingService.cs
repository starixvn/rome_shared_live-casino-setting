// --------------------------------------------------------------------------
// <copyright file="ISettingService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2014.7.21">Created</change>
// </history>

namespace Fanex.LiveCasino.CoreSetting.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fanex.LiveCasino.CoreSetting.Core.Data;
    using Fanex.LiveCasino.CoreSetting.Models;
    using Fanex.LiveCasino.CoreSetting.Criteria;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface ISettingService
    {
        Result UpdateLiveCasinoStatus();

        CustInfo GetCustomerInfo(GetCustomerInfoCriteria criteria);
    }
}
