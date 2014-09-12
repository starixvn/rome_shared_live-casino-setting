namespace Fanex.LiveCasino.CoreSetting.Web.Binders
{
    using System.Web.Mvc;
    using Fanex.Common;
    using Fanex.LiveCasino.CoreSetting.Models;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class SettingContextModelBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var request = controllerContext.HttpContext.Request;
            var settingContext = new LiveCasinoSettingContext
            {
                CustomerId = Converter.ToInt(request["customerId"]),
                ParentId = 10
            };

            return settingContext;
        }
    }
}