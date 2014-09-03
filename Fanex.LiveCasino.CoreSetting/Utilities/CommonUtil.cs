namespace Fanex.LiveCasino.CoreSetting.Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Threading;
    using System.Web;
    using Fanex.Common;
    using Fanex.LiveCasino.CoreSetting.Configuration;
    using Fanex.LiveCasino.CoreSetting.Models;
    using Fanex.Logging;

    /// <summary>
    /// Common utility class.
    /// </summary>
    public class CommonUtil : ICommonUtil
    {
        private const string __LIVE_CASINO_SETTINGS = "LiveCasinoSettings";

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

        public string IPAddress
        {
            get
            {
                HttpContext context = HttpContext.Current;

                string ipAddress = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

                if (!string.IsNullOrEmpty(ipAddress))
                {
                    string[] addresses = ipAddress.Split(',');
                    if (addresses.Length != 0)
                    {
                        return addresses[0];
                    }
                }

                return context.Request.ServerVariables["REMOTE_ADDR"];
            }
        }

        public LiveCasinoSettings LiveCasinoSettings
        {
            get
            {
                return ConfigurationManager.GetSection(__LIVE_CASINO_SETTINGS) as LiveCasinoSettings;
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

            string[] arrayCustId = custIds.Trim().Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            string[] arrayUsername = usernames.Trim().Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            string[] arraySyncStatus = syncStatus.Trim().Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

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

        public void LogInfo(string message)
        {
            Logger.Log.Info(message, LiveCasinoSettings.LogCategory);
        }

        public void LogError(string message)
        {
            Logger.Log.Error(message, LiveCasinoSettings.LogCategory);
        }
    }
}