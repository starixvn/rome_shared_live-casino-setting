namespace Fanex.LiveCasino.CoreSetting.Criteria.PtAndCom
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class SuperUpdateMemberSettingCriteria : CriteriaBase
    {
        public int CustId { get; set; }

        public decimal v162spositiontaking_1101 { get; set; }

        public bool isauto_v162spositiontaking_1101 { get; set; }

        public decimal auto_v162spositiontaking_1101 { get; set; }

        public bool v162UpdatePositiontaking_1101
        {
            get
            {
                return true;
            }
        }

        public bool v162UpdateAutoPositiontaking_1101
        {
            get
            {
                return true;
            }
        }

        public int SubAccID { get; set; }

        public string IP { get; set; }

        public int UserID { get; set; }

        public string Language { get; set; }

        public string ErrCode { get; set; }

        public string ErrMsg { get; set; }

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