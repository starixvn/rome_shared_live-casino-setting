namespace Fanex.LiveCasino.CoreSetting.Utilities
{
    using System.Collections.Generic;
    using Fanex.LiveCasino.CoreSetting.Models;

    public interface ICommonUtil
    {
        List<CustInfo> GetCustomerInfo(string custIds, string usernames, string syncStatus);
    }
}
