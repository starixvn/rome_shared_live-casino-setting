namespace Fanex.LiveCasino.CoreSetting.Criteria.PtAndCom
{
    using System.Diagnostics.CodeAnalysis;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Update Master Setting Criteria.
    /// </summary>
    public class UpdateMasterSettingCriteria : CustomerCriteriaBase
    {
        public int SRecommend { get; set; }

        [SpParam(SqlParamName = "V162spositiontaking_1101")]
        public decimal S3 { get; set; }

        [SpParam(SqlParamName = "V162mpositiontaking_1101")]
        public decimal M3 { get; set; }

        [SpParam(SqlParamName = "IsAuto_v162spositiontaking_1101")]
        public bool Is3 { get; set; }

        [SpParam(SqlParamName = "Auto_v162spositiontaking_1101")]
        public decimal Us3 { get; set; }

        [SpParam(SqlParamName = "V162mpositiontaking_1101_min")]
        public decimal N3 { get; set; }

        [SpParam(SqlParamName = "V162discount_1101")]
        public decimal MasterCommission { get; set; }

        public bool V162UpdatePositiontaking_1101
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

        public bool V162UpdateAutoPositiontaking_1101
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

        [SpParam(SqlParamName = "NewSPositionTaking")]
        public decimal NewSuperPositionTakingOnMember { get; set; }

        public int SubAccID { get; set; }

        public int UserId { get; set; }

        public string IP { get; set; }

        public string Language { get; set; }

        public override string GetSettingKey()
        {
            return "Acc_CustomerUpdMasterLiveCS_All";
        }

        public override bool IsValid()
        {
            return base.IsValid() && SRecommend > 0 && UserId > 0;
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
                "SP Name = '{0}'\nCustId = '{1}'\nSRecommend = '{2}'\nS3 = '{3}'\nM3 = '{4}'\nIs3 = '{5}'\nUs3 = '{6}'\nN3 = '{7}'\nUpdateAllMembers = '{8}'\nNewSPositionTaking = '{9}'\nSubAccID = '{10}'\nIP = '{11}'\nUserId = '{12}'\nLanguage = '{13}'",
                GetSettingKey(),
                CustId,
                SRecommend,
                S3,
                M3,
                Is3,
                Us3,
                N3,
                UpdateAllMembers,
                NewSuperPositionTakingOnMember,
                SubAccID,
                IP,
                UserId,
                Language);
        }
    }
}