namespace Fanex.LiveCasino.CoreSetting.Criteria
{
    using System.Diagnostics.CodeAnalysis;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Setting key : Acc_CustomerUpdSyncLiveCS_Cust.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed.")]
    internal class UpdateSyncLiveCasinoCriteria : CustomerCriteriaBase
    {
        public bool SyncLiveCS { get; set; }

        public int UserId { get; set; }

        public int SubAccId { get; set; }

        public string Ip { get; set; }

        public string Language { get; set; }

        public override string GetSettingKey()
        {
            return "Acc_CustomerUpdSyncLiveCS_Cust";
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
                "SP Name = '{0}'\nCustId = '{1}'\nSyncLiveCS = '{2}'\nUserID = '{3}'\nSubAccID = '{4}'\nIP = '{5}'\nLanguage = '{6}'",
                GetSettingKey(),
                CustId,
                SyncLiveCS,
                UserId,
                SubAccId,
                Ip,
                Language);
        }
    }
}