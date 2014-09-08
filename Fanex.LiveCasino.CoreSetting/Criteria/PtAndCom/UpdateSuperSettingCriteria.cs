namespace Fanex.LiveCasino.CoreSetting.Criteria.PtAndCom
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Update Super Setting Criteria.
    /// </summary>
    public class UpdateSuperSettingCriteria : CustomerCriteriaBase
    {
        [SpParam(SqlParamName = "V162spositiontaking_1101")]
        public decimal S4 { get; set; }

        [SpParam(SqlParamName = "V162spositiontaking_1101_min")]
        public decimal N4 { get; set; }

        [SpParam(SqlParamName = "V162discount_1101")]
        public decimal SuperCommission { get; set; }

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

        public bool V162UpdateCommission_1101
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
            return base.IsValid() && UserId > 0;
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
                "SP Name = '{0}'\nCustId = '{1}'\nS4 = '{2}'\nN4 = '{3}'\nSuperCommission = '{4}'\nIP = '{5}'\nUserId = '{6}'\nLanguage = '{7}'",
                GetSettingKey(),
                CustId,
                S4,
                N4,
                SuperCommission,
                IP,
                UserId,
                Language);
        }
    }
}