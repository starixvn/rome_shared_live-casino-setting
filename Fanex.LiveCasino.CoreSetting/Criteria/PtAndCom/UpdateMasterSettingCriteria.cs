namespace Fanex.LiveCasino.CoreSetting.Criteria.PtAndCom
{
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
    }
}