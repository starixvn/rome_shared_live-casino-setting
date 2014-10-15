namespace Fanex.LiveCasino.CoreSetting.Web.HtmlHelpers
{
    using System.Web.Mvc;
    using Newtonsoft.Json;

    /// <summary>
    /// JAVASCRIPT Helper for rendering JAVASCRIPT.
    /// </summary>
    public static class JavascriptHelper
    {
        /// <summary>
        /// Convert object to JAVASCRIPT object string.
        /// </summary>
        /// <param name="helper">MVC helper for inject function.</param>
        /// <param name="objConvertToJsObject">Object need be converted to JAVASCRIPT object.</param>
        /// <param name="javascriptObjectName">JAVASCRIPT Object name.</param>
        /// <returns>JAVASCRIPT object string.</returns>
        public static MvcHtmlString ToJavascriptObject(this HtmlHelper helper, object objConvertToJsObject, string javascriptObjectName)
        {
            var objectString = string.Empty;
            if (objConvertToJsObject != null)
            {
                JsonSerializerSettings settings = new JsonSerializerSettings();
                settings.NullValueHandling = NullValueHandling.Ignore;
                objectString = string.Format(
                    "var {0} = {1}; \n",
                    javascriptObjectName,
                    JsonConvert.SerializeObject(objConvertToJsObject, Formatting.Indented, settings));
            }

            return MvcHtmlString.Create(objectString);
        }
    }
}