namespace Fanex.LiveCasino.CoreSetting.Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Common;
    using Fanex.LiveCasino.CoreSetting.Models;

    /// <summary>
    /// Common utility class.
    /// </summary>
    public class CommonUtil : ICommonUtil
    {
        /// <summary>
        /// Gets the customer information.
        /// </summary>
        /// <param name="custIds">The customer ids.</param>
        /// <param name="usernames">The usernames.</param>
        /// <param name="syncStatus">The synchronize status.</param>
        /// <returns>Collection of customer information.</returns>
        public List<CustInfo> GetCustomerInfo(string custIds, string usernames, string syncStatus)
        {
            string[] arrayCustId = custIds.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            string[] arrayUsername = usernames.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            string[] arraySyncStatus = syncStatus.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            var list = new List<CustInfo>();

            for (int i = 0; i < arrayCustId.Length; i++)
            {
                list.Add(new CustInfo
                {
                    CustId = Converter.ToInt(arrayCustId[i]),
                    UserName = arrayUsername[i],
                    IsSyncLiveCasino = arraySyncStatus[i] == "1"
                });
            }

            return list;
        }
    }
}
