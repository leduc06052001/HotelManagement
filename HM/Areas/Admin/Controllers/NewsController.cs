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
            return View(new mapNews().LoadData());
        }

        //------------* ADD NEWS *------------//
        public ActionResult AddNews()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddNews(News news)
        {
            var data = new mapNews().CreateNews(news);
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
        public ActionResult UpdateNews(int ID)
        {
            return View(new mapNews().GetDetail(ID));
        }

        [HttpPost]
        public ActionResult UpdateNews(News news)
        {
            var mapped = new mapNews();
            if(mapped.UpdateNews(news) == true)
            {
                return RedirectToAction("AllNews");
            }
            else
            {
                return View(news);
            }
        }

        //------------* DELETE NEWS *------------// 
        public ActionResult DeleteNews(int ID)
        {
            new mapNews().DeleteNews(ID);
            return RedirectToAction("AllNews");
        }
    }
}