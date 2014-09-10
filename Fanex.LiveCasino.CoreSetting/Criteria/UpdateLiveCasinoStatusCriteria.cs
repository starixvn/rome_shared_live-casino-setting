namespace Fanex.LiveCasino.CoreSetting.Criteria
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Update Live Casino Status Criteria.
    /// </summary>
    internal class UpdateLiveCasinoStatusCriteria : CriteriaBase
    {
        public int CustId { get; set; }

        public bool DisabledLiveCS { get; set; }

        public bool EnableDownlinesLiveCS { get; set; }

        public int UserID { get; set; }

        public int SubAccID { get; set; }

        public string IP { get; set; }

        public string Language { get; set; }

        public override string GetSettingKey()
        {
            return "Acc_CustomerUpdDisableLiveCS";
        }

        public override bool IsValid()
        {
            return CustId > 0;
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
                "SP Name = '{0}'\nCustId = '{1}'\nDisabledLiveCS = '{2}'\nEnableDownlinesLiveCS = '{3}'\nUserID = '{4}'\nSubAccID = '{5}'\nIP = '{6}'\nLanguage = '{7}'",
                GetSettingKey(),
                CustId,
                DisabledLiveCS,
                EnableDownlinesLiveCS,
                UserID,
                SubAccID,
                IP,
                Language);
        }
    }
}