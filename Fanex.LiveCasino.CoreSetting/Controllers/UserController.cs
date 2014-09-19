namespace Fanex.LiveCasino.CoreSetting.Controllers
{
    using System.Web.Mvc;
    using Fanex.LiveCasino.CoreSetting.Configuration;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class LiveCasinoUserController : BaseController
    {
        public ActionResult UnAuthorized()
        {
            return View(LiveCasinoSettings.Settings.UnAuthorizedPath);
        }
    }
}