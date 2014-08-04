// --------------------------------------------------------------------------
// <copyright file="CustInfo.cs" company="Nexcel Solutions Vietnam">
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
    public class CustInfo
    {
        public int CustId { get; set; }

        public string UserName { get; set; }

        public string LastName { get; set; }

        public string FirstName { get; set; }

        public string NickName { get; set; }

        public string Phone { get; set; }

        public string Mobile { get; set; }

        public string Fax { get; set; }

        public byte RoleId { get; set; }

        public bool IsSyncLiveCasino { get; set; }

        public byte CurrencyId { get; set; }

        public string Site { get; set; }

        public bool DisableLiveCasino { get; set; }

        public bool UplineDisableLiveCasino { get; set; }

        public MemberWinLimit MemberWinLimit { get; set; }
    }
}