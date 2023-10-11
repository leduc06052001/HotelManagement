using HM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using DAL.Entity;
using DAL;

namespace HM.Areas.Admin.Controllers
{
    
    public class DashboardController : Controller
    {
        public ActionResult Index()
        {
            /*if (Session["manager"]  == null)
            {
                return RedirectToAction("Login");
            }
            else
            {
                return View();
            }*/
            return View();
        }

        public ActionResult Login() 
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string username, string password)
        {
            mapManager mapped = new mapManager();
            var accounts = mapped.Login(username, password);
            if(accounts != null)
            {
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.error = "*Username or Password is incorrect, please try again";
                return View();
            }
            /*if(accounts != null)
            {
                Session["manager"] = accounts;
                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }*/
        }

        public ActionResult Logout()
        {
            Session.Remove("manager");
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }


    }
}