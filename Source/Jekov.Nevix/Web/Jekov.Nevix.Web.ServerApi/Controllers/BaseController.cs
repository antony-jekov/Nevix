using Jekov.Nevix.Common.Data;
using Jekov.Nevix.Common.Data.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Jekov.Nevix.Web.ServerApi.Controllers
{
    public abstract class BaseController : Controller
    {
        protected INevixData Data { get; set; }

        public BaseController()
            : this(new NevixData())
        {
        }

        public BaseController(INevixData data)
        {
            Data = data;
        }

        public ActionResult Index()
        {
            return View();
        }
	}
}