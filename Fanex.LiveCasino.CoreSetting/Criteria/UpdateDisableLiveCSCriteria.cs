namespace Fanex.LiveCasino.CoreSetting.Criteria
{
    using System;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class UpdateDisableLiveCSCriteria : CriteriaBase
    {
        public int custid { get; set; }

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
            throw new NotImplementedException();
        }
    }
}
