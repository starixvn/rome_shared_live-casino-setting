namespace Fanex.LiveCasino.CoreSetting.Criteria.Log
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Setting key : WriteLog_UserLog_RunSP.
    /// </summary>
    public class InsertLogUserRunSPCriteria : CriteriaBase
    {
        public int UserId { get; set; }

        public int CustId { get; set; }

        public string SpName { get; set; }

        public string Doing { get; set; }

        public int ActionId { get; set; }

        public string TrackingValue { get; set; }

        public string TrackingValue2 { get; set; }

        public int Duration { get; set; }

        public override bool IsValid()
        {
            return !string.IsNullOrWhiteSpace(SpName);
        }

        public override string GetSettingKey()
        {
            return "WriteLog_UserLog_RunSP";
        }

        public override string ToString()
        {
            var template = "UserId = '{0}'\n CustId = '{1}'\n SpName = '{2}'\n ActionId = '{3}'\n OldValue = '{4}'\n NewValue = '{5}'";

            return string.Format(template, UserId, CustId, SpName, ActionId, TrackingValue.Replace("<", " < "), TrackingValue2.Replace("<", " < "));
        }
    }
}