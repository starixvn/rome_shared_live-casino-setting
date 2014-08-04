// --------------------------------------------------------------------------
// <copyright file="Commission.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2014.8.4">Created</change>
// </history>
namespace Fanex.LiveCasino.CoreSetting.Models
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class Commission
    {
        public decimal Value { get; set; }

        public Commission Upline { get; set; }
    }
}