namespace Fanex.LiveCasino.CoreSetting.Utilities
{
    using System.Collections.Generic;
    using Fanex.LiveCasino.CoreSetting.Configuration;
    using Fanex.LiveCasino.CoreSetting.Models;

    public interface ICommonUtil
    {
        string CurrentLanguage { get; }

        LiveCasinoSettings LiveCasinoSettings { get; }

        List<CustInfo> GetCustomerInfo(string custIds, string usernames, string syncStatus);
    }
}