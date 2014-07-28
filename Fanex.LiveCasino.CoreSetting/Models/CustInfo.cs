namespace Fanex.LiveCasino.CoreSetting.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

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
