using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Fanex.LiveCasino.Site.Controllers
{
    public class LiveCSSettingController : Controller
    {
        //
        // GET: /Setting/

        public ActionResult EditSetting()
        {
            return View("~/Areas/LiveCasinoSetting/Views/EditSetting/EditSetting.cshtml");
        }

    }
}
