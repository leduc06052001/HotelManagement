using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QL_khachSan.Controllers
{
    public class NewsController : Controller
    {
        //------------------* NEWS *------------------//
        public ActionResult Index()
        {
            return View(new mapNews().LoadData());
        }

        //------------------* NEWS DETAIL *------------------//
        public ActionResult NewsDetail(int ID)
        {
            return View(new mapNews().GetDetail(ID));  
        }
    }
}