namespace Fanex.LiveCasino.CoreSetting.Criteria.PtAndCom
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Update Agent Setting Criteria.
    /// </summary>
    public class UpdateAgentSettingCriteria : CriteriaBase
    {
        public int CustId { get; set; }

        public int MRecommend { get; set; }

        [SpParam(SqlParamName = "V162mpositiontaking_1101")]
        public decimal M2 { get; set; }

        [SpParam(SqlParamName = "Auto_v162mpositiontaking_1101")]
        public decimal Um2 { get; set; }

        [SpParam(SqlParamName = "IsAuto_v162mpositiontaking_1101")]
        public bool Im2 { get; set; }

        [SpParam(SqlParamName = "V162apositiontaking_1101")]
        public decimal A2 { get; set; }

        [SpParam(SqlParamName = "Min_v162apositiontaking_1101")]
        public decimal N2 { get; set; }

        [SpParam(SqlParamName = "V162discount_1101")]
        public decimal AgentCommission { get; set; }

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

        public bool V162UpdateMinPositiontaking_1101
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

        public bool UpdateAllMembers { get; set; }

        [SpParam(SqlParamName = "NewMPositionTaking")]
        public decimal NewMasterPtOnMember { get; set; }

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
            return CustId > 0 && MRecommend > 0 && UserId > 0;
        }
    }
}