namespace Fanex.LiveCasino.CoreSetting.Controllers
{
    using System.Web.Mvc;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class LiveCasinoUserController : BaseController
    {
        public ActionResult UnAuthorized()
        {
            return View("~/Areas/LiveCasinoSetting/Views/Common/UnAuthorized.cshtml");
        }
    }
}