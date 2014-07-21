// --------------------------------------------------------------------------
// <copyright file="UpdateDisableLiveCSCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2014.7.21">Created</change>
// </history>

namespace Fanex.LiveCasino.CoreSetting.Criteria
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fanex.LiveCasino.CoreSetting.Core.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class UpdateDisableLiveCSCriteria : CriteriaBase
    {
        public int custid { get; set; }

        public bool DisabledLiveCS { get; set; }

        public bool EnableDownlinesLiveCS { get; set; }

        public int UserID { get; set; }

        public int SubAccID { get; set; }

        public string IP { get; set; }

        public string Language { get; set; }

        public override string GetSettingKey()
        {
            return "Acc_CustomerUpdDisableLiveCS";
        }

        public override bool IsValid()
        {
            throw new NotImplementedException();
        }
    }
}
