using HM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace HM.Areas.Admin.Controllers
{
    public class DashboardController : Controller
    {
        HMEntities db = new HMEntities();
        public ActionResult Index()
        {
            if (Session["user"]  == null)
            {
                return RedirectToAction("Login");
            }
            else
            {
                return View();
            }
        }

        public ActionResult Login() 
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string username, string password)
        {
            var accounts = db.tb_Managers.SingleOrDefault(p=>p.UserName == username && p.Password == password);
            if(accounts != null)
            {
                Session["user"] = accounts;
                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }
        }

        public ActionResult Logout()
        {
            Session.Remove("user");
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }


    }
}