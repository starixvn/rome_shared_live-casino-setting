namespace Fanex.LiveCasino.CoreSetting.Repositories
{
    using Fanex.LiveCasino.CoreSetting.Models;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface ISettingRepository : IBaseRepository
    {
        CustInfo GetCustomerInfo(ICriteria criteria);
    }
}
