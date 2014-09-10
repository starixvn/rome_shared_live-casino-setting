namespace Fanex.LiveCasino.CoreSetting.Criteria.PtAndCom
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Base get setting for editing customer.
    /// </summary>
    internal class CustomerCriteriaBase : CriteriaBase
    {
        public int CustId { get; set; }

        public override bool IsValid()
        {
            return CustId > 0;
        }
    }
}