namespace Fanex.LiveCasino.CoreSetting.Criteria
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Result criteria.
    /// </summary>
    internal class ResultCriteria : CriteriaBase
    {
        public int? ErrCode { get; set; }

        public string ErrMsg { get; set; }
    }
}