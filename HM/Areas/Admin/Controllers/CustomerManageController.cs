using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HM.Areas.Admin.Controllers
{
    public class CustomerManageController : Controller
    {
        public ActionResult AllCustomer()
        {
            return View();
        }

        public ActionResult AddCustomer()
        {
            return View();
        }

        public ActionResult EditCustomer()
        {
            return View();
        }
    }
}