namespace Fanex.LiveCasino.CoreSetting.Criteria.PtAndCom
{
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
    }
}