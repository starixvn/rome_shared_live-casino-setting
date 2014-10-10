namespace Fanex.LiveCasino.CoreSetting.Web.Binders
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Linq;
    using System.Web.Mvc;
    using Fanex.Common;
    using Fanex.LiveCasino.CoreSetting.Enumerations;
    using Fanex.LiveCasino.CoreSetting.Models;

    /// <summary>
    /// Live casino multiple setting binder.
    /// </summary>
    public class LiveCasinoSettingContextMultipleModelBinder : IModelBinder
    {
        /// <summary>
        /// Get live casino setting multiple from request.
        /// </summary>
        /// <param name="formCollection">Form collection for reading data.</param>
        /// <returns>Live casino multiple setting context.</returns>
        public static LiveCasinoSettingContextMultiple GetLiveCasinoSettingContextMultiple(NameValueCollection formCollection)
        {
            var settingContext = new LiveCasinoSettingContextMultiple
            {
                Customer = new Customer
                {
                    Level = (CustomerLevel)Converter.ToInt(formCollection["customerLevel"]),
                    Parent = new Customer
                    {
                        Id = Converter.ToInt(formCollection["parentId"])
                    }
                },
                Upline = new Customer
                {
                    Id = Converter.ToInt(formCollection["uplineId"]),
                    Level = (CustomerLevel)Converter.ToInt(formCollection["uplineLevel"])
                },
                SubAccount = new User
                {
                    Id = Converter.ToInt(formCollection["subAccId"]),
                    Username = formCollection["subAccUsername"]
                },
                Actor = new User
                {
                    Id = Converter.ToInt(formCollection["actorId"]),
                    Username = formCollection["actorUsername"]
                },
                Language = formCollection["language"] ?? "E",
                FormType = (FormType)Converter.ToInt(formCollection["formType"], (int)FormType.Edit)
            };

            settingContext.Customers = BuildCustomers(
                formCollection["customerIds"],
                formCollection["usernames"],
                formCollection["syncStatuses"]);

            return settingContext;
        }

        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var formCollection = controllerContext.HttpContext.Request.Form;

            return GetLiveCasinoSettingContextMultiple(formCollection);
        }

        /// <summary>
        /// Build customer list from CUSTIDS, USERNAMES, SYNCSTATUS string.
        /// </summary>
        /// <param name="custIds">List of customer combined by comma.</param>
        /// <param name="usernames">List of username combined by comma.</param>
        /// <param name="syncStatus">List of sync status combined by comma.</param>
        /// <returns>Customer list.</returns>
        private static List<Customer> BuildCustomers(string custIds, string usernames, string syncStatus)
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