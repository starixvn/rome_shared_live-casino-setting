// --------------------------------------------------------------------------
// <copyright file="SpParamAttribute.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2014.7.21">Created</change>
// </history>

namespace Fanex.LiveCasino.CoreSetting.Core.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class SpParamAttribute : Attribute
    {
        public string SqlParamName { get; set; }

        public bool Ignored { get; set; }
    }
}
