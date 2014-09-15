namespace Fanex.LiveCasino.CoreSetting.Web.Binders
{
    using System.Web;
    using System.Web.Mvc;
    using Fanex.Common;
    using Fanex.LiveCasino.CoreSetting.Enumerations;
    using Fanex.LiveCasino.CoreSetting.Models;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class LiveCasinoSettingContextModelBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var request = controllerContext.HttpContext.Request;

            return GetLiveCasinoSettingContext(request);
        }

        public static LiveCasinoSettingContext GetLiveCasinoSettingContext(HttpRequestBase request)
        {
            return new LiveCasinoSettingContext
            {
                Customer = new Customer
                {
                    Id = Converter.ToInt(request["customerId"]),
                    Level = (CustomerLevel)Converter.ToInt(request["customerLevel"]),
                    Parent = new Customer
                    {
                        Id = Converter.ToInt(request["parentId"])
                    }
                },
                Upline = new Customer
                {
                    Id = Converter.ToInt(request["uplineId"]),
                    Level = (CustomerLevel)Converter.ToInt(request["uplineLevel"])
                },
                SubAccount = new User
                {
                    Id = Converter.ToInt(request["subAccId"])
                },
                Actor = new User
                {
                    Id = Converter.ToInt(request["actorId"]),
                    Username = request["actorUsername"]
                },
                Language = request["language"] ?? "E",
                FormType = (FormType)Converter.ToInt(request["formType"])
            };
        }
    }
}