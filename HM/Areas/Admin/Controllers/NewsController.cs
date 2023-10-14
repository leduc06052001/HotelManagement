using DAL;
using DAL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HM.Areas.Admin.Controllers
{
    public class NewsController : Controller
    {
        //------------* ALL NEWS *------------//
        public ActionResult AllNews()
        {
            var mapped = new mapNew();
            return View(mapped.LoadData());
        }

        //------------* ADD NEWS *------------//
        public ActionResult AddNews()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddNews(News news)
        {
            var mapped = new mapNew();
            var data = mapped.AddNews(news);
            if (data > 0)
            {
                return RedirectToAction("AllNews");
            }
            else
            {
                ModelState.AddModelError("","Please fill all infomation");
                return View(news);
            }
        }

        //------------* UPDATE NEWS *------------// 

    }
}