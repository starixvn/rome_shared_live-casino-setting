namespace Fanex.LiveCasino.CoreSetting.Criteria.GameSetting
{
    using System.Diagnostics.CodeAnalysis;
    using Fanex.LiveCasino.CoreSetting.Enumerations;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Setting key : TP_User_Customer_AutoInsertAccountFromExisting.
    /// </summary>
    internal class AutoInsertAccountFromExistingCriteria : CustomerCriteriaBase
    {
        public int OldCustId { get; set; }

        public string CasinoNo { get; set; }

        public string SignupIp { get; set; }

        public int SignupBy { get; set; }

        public int SignupFrom { get; set; }

        public string ModifyUsername { get; set; }

        public string ToUserName { get; set; }

        [SpParam(Ignored = true)]
        public CustomerLevel Level { get; set; }

        public override string GetSettingKey()
        {
            return "TP_User_Customer_AutoInsertAccountFromExisting";
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
                "SP Name = '{0}'\nCustId = '{1}'\nOldCustId = '{2}'\nCasinoNo = '{3}'\nSignupIp = '{4}'\nSignupBy = '{5}'\nSignupFrom = '{6}'\nModifyUsername = '{7}'\nToUserName = '{8}'",
                GetSettingKey(),
                CustId,
                OldCustId,
                CasinoNo,
                SignupIp,
                SignupBy,
                SignupFrom,
                ModifyUsername,
                ToUserName);
        }
    }
}