namespace Fanex.LiveCasino.CoreSetting.Models
{
    using System.Collections.Generic;
    using Fanex.LiveCasino.CoreSetting.DTO;
    using Fanex.LiveCasino.CoreSetting.Enumerations;

    /// <summary>
    /// Customer information.
    /// </summary>
    public class Customer
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

        public WinLimit MemberWinLimit { get; set; }

        public Commission Commission { get; set; }

        public PositionTaking PositionTaking { get; set; }

        public List<GameType> GameSetting { get; set; }

        public bool IsSuper
        {
            get
            {
                return RoleId == (int)CustomerLevel.Super;
            }
        }

        public bool IsMaster
        {
            get
            {
                return RoleId == (int)CustomerLevel.Master;
            }
        }

        public bool IsAgent
        {
            get
            {
                return RoleId == (int)CustomerLevel.Agent;
            }
        }

        public bool IsMember
        {
            get
            {
                return RoleId == (int)CustomerLevel.Member;
            }
        }
    }
}