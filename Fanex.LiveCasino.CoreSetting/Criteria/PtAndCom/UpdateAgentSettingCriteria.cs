namespace Fanex.LiveCasino.CoreSetting.Criteria.PtAndCom
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class UpdateAgentSettingCriteria : CriteriaBase
    {
        public int CustId { get; set; }

        public int MRecommend { get; set; }

        public decimal v162mpositiontaking_1101 { get; set; }

        public decimal auto_v162mpositiontaking_1101 { get; set; }

        public bool isauto_v162mpositiontaking_1101 { get; set; }

        public decimal v162apositiontaking_1101 { get; set; }

        public decimal min_v162apositiontaking_1101 { get; set; }

        public decimal v162discount_1101 { get; set; }

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

        public bool v162UpdateMinPositiontaking_1101
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

        public bool UpdateAllMembers { get; set; }

        public decimal NewMPositionTaking { get; set; }

        public int UserId { get; set; }

        public int SubAccID { get; set; }

        public string Language { get; set; }

        public string IP { get; set; }

        public override string GetSettingKey()
        {
            return "Acc_CustomerUpdAgentLiveCS_All";
        }

        public override bool IsValid()
        {
            return CustId > 0 && MRecommend > 0;
        }
    }
}