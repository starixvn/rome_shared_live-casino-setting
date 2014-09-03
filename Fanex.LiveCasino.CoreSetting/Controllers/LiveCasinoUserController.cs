namespace Fanex.LiveCasino.CoreSetting.Controllers
{
    using System.Web.Mvc;
    using Fanex.LiveCasino.CoreSetting.Utilities;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class LiveCasinoUserController : BaseController
    {
        public ActionResult UnAuthorized()
        {
            return View((new CommonUtil()).LiveCasinoSettings.UnAuthorizedPath);
        }
    }
}