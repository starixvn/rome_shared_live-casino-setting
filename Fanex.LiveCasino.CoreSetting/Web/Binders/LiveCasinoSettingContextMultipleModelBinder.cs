namespace Fanex.LiveCasino.CoreSetting.Web.Binders
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Fanex.Common;
    using Fanex.LiveCasino.CoreSetting.Enumerations;
    using Fanex.LiveCasino.CoreSetting.Models;
    using System.Web;
    using System.Collections.Specialized;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class LiveCasinoSettingContextMultipleModelBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var formCollection = controllerContext.HttpContext.Request.Form;

            return GetLiveCasinoSettingContextMultiple(formCollection);
        }

        public static LiveCasinoSettingContextMultiple GetLiveCasinoSettingContextMultiple(NameValueCollection formCollection)
        {
            return new LiveCasinoSettingContextMultiple
            {
                Upline = new Customer
                {
                    Id = Converter.ToInt(formCollection["uplineId"]),
                    Level = (CustomerLevel)Converter.ToInt(formCollection["uplineLevel"])
                },
                SubAccount = new User
                {
                    Id = Converter.ToInt(formCollection["subAccId"])
                },
                ActorId = Converter.ToInt(formCollection["actorId"]),
                Language = formCollection["language"],
                FormType = (FormType)Converter.ToInt(formCollection["formType"]),
                Customers = GetCustomers(formCollection["customerIds"], formCollection["customerUsernames"], formCollection["customerSyncStatuses"])
            };
        }

        private static List<Customer> GetCustomers(string custIds, string usernames, string syncStatus)
        {
            var list = new List<Customer>();

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
                    list.Add(new Customer
                    {
                        Id = Converter.ToInt(arrayCustId[i]),
                        Username = arrayUsername[i],
                        IsSyncLiveCasino = arraySyncStatus[i] == "1"
                    });
                }
            }

            return list;
        }
    }
}