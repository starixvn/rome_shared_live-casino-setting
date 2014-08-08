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
        /// Gets the customer ids.
        /// </summary>
        /// <param name="custIds">The customer ids.</param>
        /// <returns>List of customer id.</returns>
        public List<int> GetCustIds(string custIds)
        {
            var list = new List<int>();

            if (!string.IsNullOrWhiteSpace(custIds))
            {
                list = custIds.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select<string, int>(cid =>
                {
                    return Converter.ToInt(cid);
                }).ToList();
            }

            return list;
        }

        /// <summary>
        /// Get dictionary map between Customer id and username.
        /// </summary>
        /// <param name="custIds">Customer id list.</param>
        /// <param name="usernames">Username list.</param>
        /// <returns>Dictionary mapped.</returns>
        public Dictionary<int, string> GetUsernamesWithCustIds(string custIds, string usernames)
        {
            Dictionary<int, string> userNamesWithCustIds = new Dictionary<int, string>();

            string[] custIdsList = custIds.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            string[] userNameList = usernames.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            int userNameCount = userNameList.Length;
            var tempUsername = string.Empty;

            for (int i = 0; i < custIdsList.Length; i++)
            {
                tempUsername = i < userNameCount ? userNameList[i] : string.Empty;
                userNamesWithCustIds.Add(Converter.ToInt(custIdsList[i]), tempUsername);
            }

            return userNamesWithCustIds;
        }

        /// <summary>
        /// Gets the customer information.
        /// </summary>
        /// <param name="custIds">The customer ids.</param>
        /// <param name="usernames">The usernames.</param>
        /// <param name="syncStatus">The synchronize status.</param>
        /// <returns>Collection of customer information.</returns>
        public List<CustInfo> GetCustomerInfo(string custIds, string usernames, string syncStatus)
        {
            var list = new List<CustInfo>();

            if (string.IsNullOrWhiteSpace(custIds)
                || string.IsNullOrWhiteSpace(usernames)
                || string.IsNullOrWhiteSpace(syncStatus))
            {
                return list;
            }

            string[] arrayCustId = custIds.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            string[] arrayUsername = usernames.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            string[] arraySyncStatus = syncStatus.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            if (arrayCustId.Length == arrayUsername.Length
                && arrayUsername.Length == arraySyncStatus.Length
                && arrayCustId.Any())
            {
                for (int i = 0; i < arrayCustId.Length; i++)
                {
                    list.Add(new CustInfo
                    {
                        CustId = Converter.ToInt(arrayCustId[i]),
                        UserName = arrayUsername[i],
                        IsSyncLiveCasino = arraySyncStatus[i] == "1"
                    });
                }
            }

            return list;
        }
    }
}
