using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Jekov.Nevix.Web.ServerApi.Controllers
{
    public class HomeController : Controller
    {
        [OutputCache(Duration=3600, VaryByParam="none")]
        public ActionResult Index()
        {
            return View();
        }
    }
}
