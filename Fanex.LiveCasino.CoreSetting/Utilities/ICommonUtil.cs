namespace Fanex.LiveCasino.CoreSetting.Utilities
{
    using System.Collections.Generic;
    using Fanex.LiveCasino.CoreSetting.Models;

    public interface ICommonUtil
    {
        List<int> GetCustIds(string custIds);

        Dictionary<int, string> GetUsernamesWithCustIds(string custIds, string usernames);

        List<CustInfo> GetCustomerInfo(string custIds, string usernames, string syncStatus);
    }
}
