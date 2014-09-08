namespace Fanex.LiveCasino.CoreSetting.Criteria.PtAndCom
{
    using System.Diagnostics.CodeAnalysis;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Master Update Member Setting Criteria.
    /// </summary>
    public class MasterUpdateMemberSettingCriteria : CustomerCriteriaBase
    {
        [SpParam(SqlParamName = "V162mpositiontaking_1101")]
        public decimal M1 { get; set; }

        [SpParam(SqlParamName = "Auto_v162mpositiontaking_mem_1101")]
        public decimal Um1 { get; set; }

        [SpParam(SqlParamName = "IsAuto_v162mpositiontaking_mem_1101")]
        public bool Im1 { get; set; }

        [SpParam(SqlParamName = "IsAuto_v162mpositiontaking_1101")]
        public bool Im2 { get; set; }

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
            return base.IsValid() && UserId > 0;
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
                "SP Name = '{0}'\nCustId = '{1}'\nM1 = '{2}'\nUm1 = '{3}'\nIm1 = '{4}'\nIm2 = '{5}'\nUsername = '{6}'\nSubAccID = '{7}'\nUserId = '{8}'\nIP = '{9}'\nLanguage = '{10}'",
                GetSettingKey(),
                CustId,
                M1,
                Um1,
                Im1,
                Im2,
                Username,
                SubAccID,
                UserId,
                IP,
                Language);
        }
    }
}