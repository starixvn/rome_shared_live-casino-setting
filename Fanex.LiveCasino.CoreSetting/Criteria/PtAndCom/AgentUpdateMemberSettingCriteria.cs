namespace Fanex.LiveCasino.CoreSetting.Criteria.PtAndCom
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class AgentUpdateMemberSettingCriteria : CriteriaBase
    {
        public int CustId { get; set; }

        public int Recommend { get; set; }

        public decimal v162apositiontaking_1101 { get; set; }

        public decimal v162playerdiscount_1101 { get; set; }

        public bool v162UpdatePositiontaking_1101
        {
            get
            {
                return true;
            }
        }

        public bool v162UpdateCommission_1101
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

        public override string GetSettingKey()
        {
            return "Acc_CustomerUpdMemberLiveCS_All";
        }

        public override bool IsValid()
        {
            return CustId > 0 && Recommend > 0;
        }
    }
}