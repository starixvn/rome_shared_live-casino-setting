namespace Fanex.LiveCasino.CoreSetting.Models.Log
{
    using Fanex.LiveCasino.CoreSetting.Enumerations;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Log data base class.
    /// </summary>
    internal class GameSettingLog : IGameSettingLog
    {
        private const string TrackingSubAcc = "SubAccId=\"{0}\" SubAccName=\"{1}\"";

        public virtual ICriteria Criteria { get; set; }

        public virtual LogActions Action { get; set; }

        public virtual string NewValue { get; set; }

        public virtual string OldValue { get; set; }

        public virtual string SpName { get; set; }

        public string ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public string GetTrackingSubAccount(int roleId, int subAccountId, string subAccountUsername)
        {
            var tracking = string.Empty;

            if (roleId == (int)CustomerLevel.Super)
            {
                tracking = string.Format(TrackingSubAcc, subAccountId, subAccountUsername);
            }

            return tracking;
        }
    }
}