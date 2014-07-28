namespace Fanex.LiveCasino.CoreSetting.Criteria.PtAndCom
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class MasterUpdateMemberSettingCriteria : CriteriaBase
    {
        public int CustId { get; set; }

        public decimal v162mpositiontaking_1101 { get; set; }

        public decimal auto_v162mpositiontaking_mem_1101 { get; set; }

        public bool isauto_v162mpositiontaking_mem_1101 { get; set; }

        public bool isauto_v162mpositiontaking_1101 { get; set; }

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
            return CustId > 0;
        }
    }
}