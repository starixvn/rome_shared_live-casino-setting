namespace Fanex.LiveCasino.CoreSetting.Criteria.PtAndCom
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Master Update Member Setting Criteria.
    /// </summary>
    public class MasterUpdateMemberSettingCriteria : CriteriaBase
    {
        public int CustId { get; set; }

        public decimal V162mpositiontaking_1101 { get; set; }

        public decimal Auto_v162mpositiontaking_mem_1101 { get; set; }

        public bool IsAuto_v162mpositiontaking_mem_1101 { get; set; }

        public bool IsAuto_v162mpositiontaking_1101 { get; set; }

        public bool V162UpdatePositiontaking_1101
        {
            get
            {
                return true;
            }
        }

        public bool V162UpdateAutoPositiontaking_1101
        {
            get
            {
                return true;
            }
        }

        public int UserId { get; set; }

        public int SubAccID { get; set; }

        public string Language { get; set; }

        public string IP { get; set; }

        public string Username { get; set; }

        public override string GetSettingKey()
        {
            return "Acc_CustomerUpdMemberLiveCS_MAg";
        }

        public override bool IsValid()
        {
            return CustId > 0 & UserId > 0;
        }
    }
}