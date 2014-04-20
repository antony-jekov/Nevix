using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Jekov.Nevix.Web.ServerApi.Controllers
{
    public class InfoController : Controller
    {
        //[OutputCache(CacheProfile = "Cache1Hour")]
        public ActionResult GettingStarted()
        {
            return View();
        }

        public ActionResult Index()
        {
            return View();
        }
	}
}