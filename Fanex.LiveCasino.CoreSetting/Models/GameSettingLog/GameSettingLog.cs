namespace Fanex.LiveCasino.CoreSetting.Models.GameSettingLog
{
    using Fanex.LiveCasino.CoreSetting.Enumerations;

    /// <summary>
    /// Log data base class.
    /// </summary>
    public class GameSettingLog : IGameSettingLog
    {
        private const string TrackingSubAcc = "SubAccId=\"{0}\" SubAccName=\"{1}\"";

        public GameSettingLog()
        {
            SettingContext = new SettingContext();
        }

        public virtual LogActions Action { get; set; }

        public SettingContext SettingContext { get; set; }

        public string CopiedFrom { get; set; }

        public string ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public virtual string NewValue { get; set; }

        public virtual string OldValue { get; set; }

        public virtual string SpName { get; set; }

        public string TrackingSubAccount
        {
            get
            {
                var tracking = string.Empty;

                if (SettingContext.CustomerLevel == CustomerLevel.Super)
                {
                    tracking = string.Format(TrackingSubAcc, SettingContext.SubAccId, SettingContext.SubAccName);
                }

                return tracking;
            }
        }
    }
}