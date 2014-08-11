namespace Fanex.LiveCasino.CoreSetting.Utilities
{
    using System.Collections.Generic;
    using Fanex.LiveCasino.CoreSetting.Models;

    public interface ICommonUtil
    {
        string CurrentLanguage { get; }

        List<CustInfo> GetCustomerInfo(string custIds, string usernames, string syncStatus);
    }
}
