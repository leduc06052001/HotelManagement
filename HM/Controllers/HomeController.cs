using DAL;
using DAL.Entity;
using Facebook;
using HM.Common;
using DAL.Entity.Models;
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
using System.Web.Helpers;

namespace HM.Controllers
{
    public class HomeController : Controller
    {
        HMEntities db = new HMEntities();

        //------------------* REDIRECT URI(FOR FACEBOOK) *------------------//
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

        //------------------* REGISTER *------------------//
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

        //------------------* LOGIN *------------------//
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string email, string password)
        {
            var login = new mapCustomer().Login(email, password);
            if (login != null)
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

        //------------------* LOGIN WITH FACEBOOK *------------------//
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

        //------------------* FACEBOOK CALLBACK *------------------//
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

        //------------------* FORGOT PASSWORD *------------------//

        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ForgotPassword(ForgotPasswordViewModel forgotPassword)
        {
            if (ModelState.IsValid)
            {
                var message = "";
                var userData = db.Customers.SingleOrDefault(p => p.Email == forgotPassword.Email);
                if (userData != null)
                {
                    // Tạo mã token đặt lại mật khẩu
                    // Tạo liên kết đặt lại mật khẩu (có bao gồm token)
                    // Gửi liên kết đến email đã nhập (có kiểm tra db)
                    // Gán mã token vừa tạo vào db
                    // Lưu thay đổi

                    string resetToken = Guid.NewGuid().ToString();
                    var resetLink = Url.Action("ResetPassword", "Home", new { email = forgotPassword.Email, code = resetToken }, protocol: Request.Url.Scheme);
                    new MailHelper().SendEmail("Artemis Hotel", forgotPassword.Email, "Reset password", "You are requeting reset password for your account. Please <a href = '" + resetLink + "'>click here</a> to reset password");
                    userData.resetPasswordCode = resetToken;
                    db.Configuration.ValidateOnSaveEnabled = false;
                    db.SaveChanges();
                    return RedirectToAction("ForgotPasswordConfimation");
                }
                else
                {
                    message = "Email not found. Please check again...";
                }
                ViewBag.message = message;
            }
            return View(forgotPassword);
        }

        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            // Xác minh liên kết đặt lại mật khẩu
            // Xác định tài khoản phù hợp với liên kết đặt lại mật khẩu
            // Chuyển hướng đến trang đặt lại mật khẩu 
            var userData = db.Customers.Where(p => p.resetPasswordCode == code).FirstOrDefault();
            if (userData != null)
            {
                ResetPasswordViewModel model = new ResetPasswordViewModel();
                model.ResetPasswordCode = code;
                return View(model);
            }
            return HttpNotFound();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(ResetPasswordViewModel resetPassword)
        {
            var message = "";
            if (ModelState.IsValid)
            {
                // Kiểm tra mã đặt lại mật khẩu
                // True: thay đổi bằng mật khẩu mới (sử dụng hàm băm)
                // Lưu thay đổi
                var userData = db.Customers.Where(p => p.resetPasswordCode == resetPassword.ResetPasswordCode).FirstOrDefault();
                if (userData != null)
                {
                    userData.Password = Crypto.Hash(resetPassword.NewPassword);
                    userData.resetPasswordCode = "";
                    db.Configuration.ValidateOnSaveEnabled = false;
                    db.SaveChanges();
                    return RedirectToAction("ForgotPasswordSuccess");
                }
            }

            ViewBag.message = message;
            return View();
        }

        public ActionResult ForgotPasswordConfimation()
        {
            return View();
        }

        public ActionResult ForgotPasswordSuccess()
        {
            return View();
        }
    }
}