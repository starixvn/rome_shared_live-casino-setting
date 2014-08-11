namespace Fanex.LiveCasino.CoreSetting.Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using Fanex.Common;
    using Fanex.LiveCasino.CoreSetting.Models;

    /// <summary>
    /// Common utility class.
    /// </summary>
    public class CommonUtil : ICommonUtil
    {
        public string CurrentLanguage
        {
            get
            {
                switch (Thread.CurrentThread.CurrentCulture.Name)
                {
                    case "en-US": return "en";
                    case "zh-TW": return "ch"; // Traditional
                    case "zh-CN": return "cs"; // Simplified
                    case "ja-JP": return "jp"; // Japan
                    case "th-TH": return "th"; // Thai
                    case "ko-KR": return "ko"; // Korean
                    case "vi-VN": return "vn"; // Vietnamese
                    default: return "en";
                }
            }
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
