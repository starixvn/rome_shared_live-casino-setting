namespace Fanex.LiveCasino.CoreSetting.Criteria.Log
{
    using Fanex.NAP.Framework.Data;

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
    }
}
