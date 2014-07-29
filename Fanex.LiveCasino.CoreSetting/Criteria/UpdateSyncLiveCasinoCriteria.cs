namespace Fanex.LiveCasino.CoreSetting.Criteria
{
    using Fanex.NAP.Framework.Data;

    public class UpdateSyncLiveCasinoCriteria : CriteriaBase
    {
        public int CustId { get; set; }

        public bool SyncLiveCS { get; set; }

        public int UserId { get; set; }

        public int SubAccId { get; set; }

        public string Ip { get; set; }

        public string Language { get; set; }

        public override bool IsValid()
        {
            return CustId > 0;
        }

        public override string GetSettingKey()
        {
            return "Acc_CustomerUpdSyncLiveCS";
        }
    }
}
