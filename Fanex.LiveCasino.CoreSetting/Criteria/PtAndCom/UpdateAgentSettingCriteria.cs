namespace Fanex.LiveCasino.CoreSetting.Criteria.PtAndCom
{
    using System.Diagnostics.CodeAnalysis;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Update Agent Setting Criteria.
    /// </summary>
    internal class UpdateAgentSettingCriteria : CustomerCriteriaBase
    {
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
            return base.IsValid() && MRecommend > 0 && UserId > 0;
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules", "SP2100:CodeLineMustNotBeLongerThan", Justification = "Reviewed. Suppression is OK here.")]
        public override string ToString()
        {
            return string.Format(
                "SP Name = '{0}'\nCustId = '{1}'\nMRecommend = '{2}'\nM2 = '{3}'\nUm2 = '{4}'\nIm2 = '{5}'\nA2 = '{6}'\nN2 = '{7}'\nAgentCommission = '{8}'\nNewMPositionTaking = '{9}'\nSubAccID = '{10}'\nIP = '{11}'\nActorId = '{12}'\nLanguage = '{13}'",
                GetSettingKey(),
                CustId,
                MRecommend,
                M2,
                Um2,
                Im2,
                A2,
                N2,
                AgentCommission,
                NewMasterPtOnMember,
                SubAccID,
                IP,
                UserId,
                Language);
        }
    }
}