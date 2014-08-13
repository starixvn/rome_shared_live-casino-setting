namespace Fanex.LiveCasino.CoreSetting.Models.Log
{
    using Fanex.LiveCasino.CoreSetting.Enumerations;

    /// <summary>
    /// Log data base class.
    /// </summary>
    public class LogDataBase : ILogData
    {
        protected const string TrackingSubAcc = "SubAccId=\"{0}\" SubAccName=\"{1}\"";

        public LogDataBase()
        {
            CustInfo = new CustInfo();
            SubAccount = new CustInfo();
            Modifier = new CustInfo();
        }

        public LogActions Action { get; set; }

        public CustInfo CustInfo { get; set; }

        public CustInfo SubAccount { get; set; }

        public CustInfo Modifier { get; set; }

        public string CopiedFrom { get; set; }

        public string ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public string Language { get; set; }

        public virtual string NewValue { get; set; }

        public virtual string OldValue { get; set; }

        public string SignUpIp { get; set; }

        public virtual string SpName { get; set; }

        public string TrackingSubAccount
        {
            get
            {
                var tracking = string.Empty;

                if (CustInfo != null
                    && SubAccount != null
                    && CustInfo.RoleId == (int)CustRole.Super)
                {
                    tracking = string.Format(TrackingSubAcc, SubAccount.CustId, SubAccount.UserName);
                }

                return tracking;
            }
        }
    }
}