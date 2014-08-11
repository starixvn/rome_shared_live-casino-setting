namespace Fanex.LiveCasino.CoreSetting.Criteria.PtAndCom
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Agent Update Member Setting Criteria.
    /// </summary>
    public class AgentUpdateMemberSettingCriteria : CriteriaBase
    {
        public int CustId { get; set; }

        public int Recommend { get; set; }

        [SpParam(SqlParamName = "V162apositiontaking_1101")]
        public decimal A1 { get; set; }

        [SpParam(SqlParamName = "V162playerdiscount_1101")]
        public decimal MemberCommission { get; set; }

        public bool V162UpdatePositiontaking_1101
        {
            get
            {
                return true;
            }
        }

        public bool V162UpdateCommission_1101
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
            return "Acc_CustomerUpdMemberLiveCS_All";
        }

        public override bool IsValid()
        {
            return CustId > 0 && Recommend > 0 && UserId > 0;
        }
    }
}