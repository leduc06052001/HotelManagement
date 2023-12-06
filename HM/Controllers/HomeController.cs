using DAL;
using DAL.Entity;
using Facebook;
using HM.Common;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace QL_khachSan.Controllers
{
    public class HomeController : Controller
    {
        HMEntities db = new HMEntities();

        //Redirect Uri (for facebook -->)
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
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(Customer customer)
        {
            db.Customers.Add(customer);
            db.SaveChanges();
            return RedirectToAction("Login");
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string email, string password)
        {
            var login = new mapCustomer().Login(email, password);
            if(login != null)
            {
                var userSession = new UserLogin();
                userSession.UserName = login.FullName;
                userSession.UserID = login.CustomerID;
                userSession.Image = login.Image;
                Session.Add(CommonConstants.USER_SESSION, userSession);
                return RedirectToAction("Index");
            }
            return View();
        }

        // Login with Facebook
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
                string firstName = me.first_name;
                string middleName = me.middle_name;
                string lastName = me.last_name;

                var user = new Customer();
                user.Email = email;
                user.FullName = firstName + " " + middleName + " " + lastName;
                var resultInsert = new mapCustomer().InsertForFacebook(user);
                if (resultInsert > 0)
                {
                    var userSession = new UserLogin();
                    userSession.UserName = user.FullName;
                    userSession.UserID = user.CustomerID;
                    Session.Add(CommonConstants.USER_SESSION, userSession);
                }
            }
            return Redirect("/");
        }


        //--->>> Login with google
        /*//lấy id client từ webconfig
        string clientId = ConfigurationManager.AppSettings["GgAppId"];
        string redirection_url = ConfigurationManager.AppSettings["redirectUri"];

        public void LoginGoogle()
        {
            string urls = "https://accounts.google.com/o/oauth2/v2/auth?scope=email&include_granted_scopes=true&redirect_uri=" + redirection_url + "&response_type=code&client_id=" + clientId + "";
            Response.Redirect(urls);
        }*/

        public ActionResult Logout()
        {
            Session.Remove("USER_SESSION");
            FormsAuthentication.SignOut();
            return RedirectToAction("Index");
        }

        public ActionResult ForgotPassword()
        {
            return View();
        }

}