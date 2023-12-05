using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using DAL.Entity;
using DAL;
using HM.App_Start;
using Facebook;
using System.Configuration;
using HM.Areas.Admin.Models;
using Microsoft.AspNet.Identity;

namespace HM.Areas.Admin.Controllers
{

    public class DashboardController : Controller
    {
        [RoleAdmin]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }
       
        [HttpPost]
        public ActionResult Login(string username, string password)
        {
            var accounts = new mapManager().Login(username, password);
            if (accounts != null)
            {
                AdminSessionConfig.SetUser(accounts);
                AdminSessionConfig.GetUser();
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.error = "*Tên đăng nhập hoặc mật khẩu không đúng";
                return View("Login");
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

        //----- ĐĂNG KÝ TÀI KHOẢN -----//
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(Manager managers)
        {
            var accounts = new mapManager().Register(managers);
            if (accounts > 0)
            {
                return RedirectToAction("Login");
            }
            return View();
        }
    }
}