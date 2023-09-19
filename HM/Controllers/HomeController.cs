using HM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace QL_khachSan.Controllers
{
    public class HomeController : Controller
    {
        HMEntities db = new HMEntities();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(tb_Customers customer)
        {
            db.tb_Customers.Add(customer);
            db.SaveChanges();
            return RedirectToAction("Login");
        }

        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(tb_Customers customer)
        {
            var emailForm = customer.Email;
            var passwordForm = customer.Password;
            var login = db.tb_Customers.SingleOrDefault(p => p.Email.Equals(emailForm) && p.Password == passwordForm);
            if(login != null)
            {
                Session["user"] = login;
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