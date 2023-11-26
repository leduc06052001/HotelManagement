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
        private Uri RedirectUri
        {
            get
            {
                var uriBuilder = new UriBuilder(Request.Url);
                uriBuilder.Query = null;
                uriBuilder.Fragment = null;
                uriBuilder.Path = Url.Action("FacebookCallback");
                return uriBuilder.Uri;
            }
        }
        [RoleUser]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        public ActionResult LoginFacebook()
        {
            var fb = new FacebookClient();
            var loginUrl = fb.GetLoginUrl(new
            {
                client_id = ConfigurationManager.AppSettings["FbAppId"],
                client_secret = ConfigurationManager.AppSettings["FbAppSecret"],
                redirect_uri = RedirectUri.AbsoluteUri,
                response_type = "code",
                scope = "email"
            });
            return Redirect(loginUrl.AbsoluteUri);
        }

        //-----> FacebookCallBack
        public ActionResult FacebookCallBack(string code)
        {
            var fb = new FacebookClient();
            dynamic result = fb.Post("oauth/access_token", new
            {
                client_id = ConfigurationManager.AppSettings["FbAppId"],
                client_secret = ConfigurationManager.AppSettings["FbAppSecret"],
                redirect_uri = RedirectUri.AbsoluteUri,
                code = code
            });
            var accessToken = result.access_token;
            if (!string.IsNullOrEmpty(accessToken))
            {
                fb.AccessToken = accessToken;
                //Lấy thông tin người dùng (tên, email,...)
                dynamic me = fb.Get("me?fields=first_name, middle_name, last_name,id,email");
                string email = me.email;
                string userName = me.email;
                string firstName = me.first_name;
                string middleName = me.middle_name;
                string lastName = me.last_name;
                string phoneNumber = me.phone_number;

                var user = new Manager();
                user.Email = email;
                user.UserName = email;
                user.FullName = firstName + " " + middleName + " " + lastName;
                var resultInsert = new mapManager().InsertForFacebook(user);
                if(resultInsert > 0)
                {
                    
                   
                }
            } 
            else
            {

            }
        }

        [HttpPost]
        public ActionResult Login(string username, string password)
        {
            var accounts = new mapManager().Login(username, password);
            if (accounts != null)
            {
                SessionConfig.SetUser(accounts);
                SessionConfig.GetUser();
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

        public ActionResult LoginGoogle()
        {
            return View();
        }

        public ActionResult Logout()
        {
            Session.Remove("manager");
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }

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