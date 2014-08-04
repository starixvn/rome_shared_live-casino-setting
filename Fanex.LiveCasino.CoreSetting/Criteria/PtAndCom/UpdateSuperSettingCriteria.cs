namespace Fanex.LiveCasino.CoreSetting.Criteria.PtAndCom
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class UpdateSuperSettingCriteria : CriteriaBase
    {
        public int CustId { get; set; }

        public decimal v162spositiontaking_1101 { get; set; }

        public decimal v162spositiontaking_1101_min { get; set; }

        public decimal v162discount_1101 { get; set; }

        public bool v162UpdatePositiontaking_1101
        {
            get
            {
                return true;
            }
        }

        public bool v162UpdateMinPositiontaking_1101
        {
            get
            {
                return true;
            }
        }

        public bool v162UpdateCommission_1101
        {
            get
            {
                return true;
            }
        }

        public string IP { get; set; }

        public int UserId { get; set; }

        public string Language { get; set; }

        public override string GetSettingKey()
        {
            return "Acc_CustomerUpdSuperLiveCS_All";
        }

        public override bool IsValid()
        {
            return CustId > 0 && UserId > 0;
        }
    }
}