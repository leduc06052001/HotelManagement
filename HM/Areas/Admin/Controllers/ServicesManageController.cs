using DAL;
using DAL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HM.Areas.Admin.Controllers
{
    public class ServicesManageController : Controller
    {
        public ActionResult AllServices()
        {
            return View(new mapServices().LoadData());
        }

        //------------------* Create *------------------//
        public ActionResult CreateServices()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateServices(Service services)
        {
            var data = new mapServices().AddServices(services);
            if(data > 0)
            {
                return RedirectToAction("AllServices");
            }
            else
            {
                return View(services);
            }
        }

        //------------------* Update *------------------//
        public ActionResult UpdateServices(int id)
        {
            return View(new mapServices().GetDetail(id));
        }

        [HttpPost]
        public ActionResult UpdateServices(Service services)
        {
            var data = new mapServices().UpdateServices(services);
            if(data == true)
            {
                return RedirectToAction("AllServices");
            }
            return View(services);
        }

        //------------------* Delete *------------------//
        public ActionResult DeleteServices(int id)
        {
            new mapServices().DeleteServices(id);
            return RedirectToAction("AllService");
        }
    }
}