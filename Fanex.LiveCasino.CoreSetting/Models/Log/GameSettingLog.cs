namespace Fanex.LiveCasino.CoreSetting.Models.Log
{
    using Fanex.LiveCasino.CoreSetting.Enumerations;

    /// <summary>
    /// Log data base class.
    /// </summary>
    internal class GameSettingLog : IGameSettingLog
    {
        private const string TrackingSubAcc = "SubAccId=\"{0}\" SubAccName=\"{1}\"";

        public virtual LiveCasinoSettingContext SettingContext { get; set; }

        public virtual LogActions Action { get; set; }

        public virtual string NewValue { get; set; }

        public virtual string OldValue { get; set; }

        public virtual string SpName { get; set; }

        public string ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public string TrackingSubAccount
        {
            get
            {
                var tracking = string.Empty;

                if (SettingContext.SubAccount.Id > 0)
                {
                    tracking = string.Format(TrackingSubAcc, SettingContext.SubAccount.Id, SettingContext.SubAccount.Username);
                }

                return tracking;
            }
        }
    }
}