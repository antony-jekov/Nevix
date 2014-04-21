using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Jekov.Nevix.Web.ServerApi.Controllers
{
    public class InfoController : Controller
    {
        [OutputCache(Duration = 3600, VaryByParam = "none")]
        public ActionResult GettingStarted()
        {
            return View();
        }

        public ActionResult Features()
        {
            return View();
        }

        public ActionResult Index()
        {
            return View();
        }
	}
}