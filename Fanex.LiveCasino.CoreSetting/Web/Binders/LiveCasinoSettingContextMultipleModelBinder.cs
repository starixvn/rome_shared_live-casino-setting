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
    using Fanex.LiveCasino.CoreSetting.Services;

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
                    Id = Converter.ToInt(formCollection["subAccId"])
                },
                Actor = new User
                {
                    Id = Converter.ToInt(formCollection["actorId"]),
                    Username = formCollection["actorUsername"]
                },
                Language = formCollection["language"] ?? "E",
                FormType = (FormType)Converter.ToInt(formCollection["formType"])
            };

            settingContext.Customers = SettingService.BuildCustomers(
                formCollection["customerIds"],
                formCollection["usernames"],
                formCollection["syncStatuses"]);

            return settingContext;
        }
    }
}