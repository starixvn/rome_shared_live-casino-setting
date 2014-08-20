namespace Fanex.LiveCasino.CoreSetting.Criteria.PtAndCom
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Super Update Member Setting Criteria.
    /// </summary>
    public class SuperUpdateMemberSettingCriteria : CustomerCriteriaBase
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
    }
}