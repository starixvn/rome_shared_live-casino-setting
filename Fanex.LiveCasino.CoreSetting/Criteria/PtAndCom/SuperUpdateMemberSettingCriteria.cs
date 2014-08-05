namespace Fanex.LiveCasino.CoreSetting.Criteria.PtAndCom
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Super Update Member Setting Criteria.
    /// </summary>
    public class SuperUpdateMemberSettingCriteria : CriteriaBase
    {
        public int CustId { get; set; }

        public decimal V162spositiontaking_1101 { get; set; }

        public bool IsAuto_v162spositiontaking_1101 { get; set; }

        public decimal Auto_v162spositiontaking_1101 { get; set; }

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

        public int SubAccID { get; set; }

        public string IP { get; set; }

        public int UserId { get; set; }

        public string Language { get; set; }

        public override string GetSettingKey()
        {
            return "Acc_CustomerUpdMemberLiveCS_SAg";
        }

        public override bool IsValid()
        {
            return CustId > 0;
        }
    }
}