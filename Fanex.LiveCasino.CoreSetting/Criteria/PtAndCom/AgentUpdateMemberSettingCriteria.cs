namespace Fanex.LiveCasino.CoreSetting.Criteria.PtAndCom
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Agent Update Member Setting Criteria.
    /// </summary>
    public class AgentUpdateMemberSettingCriteria : CustomerCriteriaBase
    {
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
            return base.IsValid() && Recommend > 0 && UserId > 0;
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format(
                "SP Name = '{0}'\nCustId = '{1}'\nRecommend = '{2}'\nMemberCommission = '{3}'\nSubAccID = '{4}'\nUserId = '{5}'\nIP = '{6}'\nLanguage = '{7}'",
                GetSettingKey(),
                CustId,
                Recommend,
                MemberCommission,
                SubAccID,
                UserId,
                IP,
                Language);
        }
    }
}