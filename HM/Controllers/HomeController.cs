using DAL;
using DAL.Entity;
using Facebook;
using HM.Common;
using System;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.Helpers;
using System.Web.UI.WebControls;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNet.Identity;
using DAL.Entity.Models;

namespace HM.Controllers
{

    public class HomeController : Controller
    {
        HMEntities db = new HMEntities();

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
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterViewModel registerModel)
        {
            if (ModelState.IsValid)
            {
                var currentCustomer = db.Customers.Where(p => p.Email == registerModel.Email).FirstOrDefault();
                if (currentCustomer == null)
                {
                    db.Customers.Add(currentCustomer);
                    db.SaveChanges();
                    return RedirectToAction("Login");
                }
                else
                {
                    ModelState.AddModelError("Email", "Email này đã đăng ký tài khoản, vui lòng đăng nhập");
                    return View(registerModel);
                }
            }
            else
            {
                return View(registerModel);
            }
        }

        //------------------* LOGIN *------------------//
        public ActionResult Login(string returnUrl)
        {
            Session["returnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel loginModel)
        {
            if (ModelState.IsValid)
            {
                var login = db.Customers.FirstOrDefault(p => p.Email == loginModel.Email);
                if (login == null)
                {
                    ModelState.AddModelError("Email", "Email không tồn tại, hãy đăng ký nhé.");
                    return View(loginModel);
                }
                else if (login.Password != loginModel.Password)
                {
                    ModelState.AddModelError("Password", "Mật khẩu không đúng");
                    return View(loginModel);
                }

                var userSession = new Customer();
                userSession.CustomerID = login.CustomerID;
                userSession.FullName = login.FullName;
                userSession.Address = login.Address;
                userSession.Phone = login.Phone;
                userSession.IdentifyNumber = login.IdentifyNumber;
                userSession.Email = login.Email;
                userSession.Gender = login.Gender;
                userSession.BirthDate = login.BirthDate;
                userSession.Image = login.Image;
                Session["user"] = userSession;
                string returnUrl = Session["returnUrl"] as string;
                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                {
                    Session.Remove("returnUrl"); //Xóa returnUrl sau khi sử dụng
                    return Redirect(returnUrl);
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }
            return View(loginModel);

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
                    var userSession = new Customer();
                    userSession.CustomerID = user.CustomerID;
                    userSession.FullName = user.FullName;
                    userSession.Address = user.Address;
                    userSession.Phone = user.Phone;
                    userSession.IdentifyNumber = user.IdentifyNumber;
                    userSession.Email = user.Email;
                    userSession.Gender = user.Gender;
                    userSession.BirthDate = user.BirthDate;
                    userSession.Image = user.Image;
                    Session["user"] = userSession;
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
            Session.Remove("user");
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
                var userData = db.Customers.FirstOrDefault(p => p.Email == forgotPassword.Email);
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
                    ModelState.AddModelError("Email", "Email này chưa đăng ký tài khoản.");
                    return View(forgotPassword);
                }
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

        public ActionResult ProfileCustomer()
        {
            var user = (Customer)Session["user"];
            return View(user);
        }
        //------------------* CHECK CHECKAVAILABLE *------------------//
        /*        public ActionResult CheckAvailable(string roomType, int adult, int child)
                {
                    IQueryable<Room> rooms = db.Rooms;
                    if (!string.IsNullOrEmpty(roomType))
                    {

                    }
                    return View();
                }*/

    }
}