namespace Fanex.LiveCasino.CoreSetting.Criteria.PtAndCom
{
    using System.Diagnostics.CodeAnalysis;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Super Update Member Setting Criteria.
    /// </summary>
    internal class SuperUpdateMemberSettingCriteria : CustomerCriteriaBase
    {
        [SpParam(SqlParamName = "V162spositiontaking_1101")]
        public decimal S1 { get; set; }

        [SpParam(SqlParamName = "IsAuto_v162spositiontaking_1101")]
        public bool Is1 { get; set; }

        [SpParam(SqlParamName = "Auto_v162spositiontaking_1101")]
        public decimal Us1 { get; set; }

        [SpParam(SqlParamName = "V162UpdatePositiontaking_1101")]
        public bool IsUpdateSuperPt { get; set; }

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
                "SP Name = '{0}'\nCustId = '{1}'\nS1 = '{2}'\nIs1 = '{3}'\nUs1 = '{4}'\nIsUpdateSuperPt = '{5}'\nSubAccID = '{6}'\nActorId = '{7}'\nIP = '{8}'\nLanguage = '{9}'",
                GetSettingKey(),
                CustId,
                S1,
                Is1,
                Us1,
                IsUpdateSuperPt,
                SubAccID,
                UserId,
                IP,
                Language);
        }
    }
}