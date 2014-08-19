namespace Fanex.LiveCasino.CoreSetting.Web.Filters
{
    using System.IO;
    using System.Web;
    using System.Web.Mvc;
    using Newtonsoft.Json;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class CustomJsonModelBinder<T> : DefaultModelBinder where T : class
    {
        /// <summary>
        /// Binds the model by using the specified controller context and binding context.
        /// </summary>
        /// <returns>
        /// The bound object.
        /// </returns>
        /// <param name="controllerContext">The context within which the controller operates. The context information includes the controller, HTTP content, request context, and route data.</param><param name="bindingContext">The context within which the model is bound. The context includes information such as the model object, model name, model type, property filter, and value provider.</param><exception cref="T:System.ArgumentNullException">The <paramref name="bindingContext "/>parameter is null.</exception>
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            HttpRequestBase request = controllerContext.HttpContext.Request;
            request.InputStream.Position = 0;
            var input = new StreamReader(request.InputStream).ReadToEnd();
            T modelObject = JsonConvert.DeserializeObject<T>(input);
            return modelObject;
        }
    }
}