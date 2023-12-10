using Antlr.Runtime.Tree;
using DAL.Entity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Management;
using System.Web.Mvc;

namespace HM.Controllers
{

    public class BookingsController : Controller
    {
        HMEntities db = new HMEntities();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Booking(int ID)
        {
            var room = db.Rooms.Find(ID);
            if (room == null)
            {
                return HttpNotFound();
            }
            var booking = new Booking
            {
                RoomID = room.RoomID,
                CheckinDate = DateTime.Today,
                CheckoutDate = DateTime.Today.AddDays(1),
                BookingDate = DateTime.UtcNow
            };

            ViewBag.RoomDetail = room;
            return View(booking);
        }

        [HttpPost]
        public ActionResult Booking(Booking booking)
        {
            if (ModelState.IsValid)
            {
                var room = db.Rooms.Find(booking.RoomID);
                if (room == null)
                {
                    return HttpNotFound();
                }
                booking.Room = room;
                booking.BookingDate = DateTime.UtcNow;
                if (booking.CheckinDate.HasValue && booking.CheckoutDate.HasValue)
                {
                    TimeSpan timeSpan = booking.CheckoutDate.Value - booking.CheckinDate.Value;
                    booking.TotalAmount = (int)timeSpan.TotalDays * booking.Room.Price;
                }
                db.Bookings.Add(booking);
                db.SaveChanges();
                SendInfoBookingToCustomer(booking);
                /*string content = System.IO.File.ReadAllText(Server.MapPath("~/Content/templateEmail/SendEmailtoCustomer.html"));
                content = content.Replace("{{CustomerName}}", booking.CustomerName);
                content = content.Replace("{{Email}}", booking.Email);
                content = content.Replace("{{Phone}}", booking.Phone);
                content = content.Replace("{{RoomType}}", booking.Room.RoomType.RoomTypeName);
                content = content.Replace("{{Adult}}", booking.Adult.ToString());
                content = content.Replace("{{Child}}", booking.Child.ToString());
                content = content.Replace("{{BookingDate}}", booking.BookingDate.ToString());
                content = content.Replace("{{Note}}", booking.Note.ToString());
                content = content.Replace("{{TotalAmount}}", booking.TotalAmount.ToString());

                var toEmail = booking.Email.ToString();
                var toAdmin = ConfigurationManager.AppSettings["ToEmailAddress"].ToString();
                new MailHelper().SendEmail(toAdmin, "Đơn hàng mới", content);
                new MailHelper().SendEmail(toEmail, "Artemis Hotel", content);*/
                return Redirect("~/Home/Index");
            }
            else
            {
                if (ModelState["CustomerName"].Errors.Count > 0)
                {
                    ViewBag.ErrorMessage = "Vui lòng nhập tên";
                }
                if (ModelState["Email"].Errors.Count > 0)
                {
                    ViewBag.ErrorMessage = "Vui lòng nhập Email";
                }
                if (ModelState["CheckinDate"].Errors.Count > 0)
                {
                    ViewBag.ErrorMessage = "Vui lòng chọn ngày nhận phòng";
                }
                if (ModelState["CheckoutDate"].Errors.Count > 0)
                {
                    ViewBag.ErrorMessage = "Vui lòng chọn ngày trả phòng";
                }
                if (ModelState["Phone"].Errors.Count > 0)
                {
                    ViewBag.ErrorMessage = "Vui lòng nhập số điện thoại";
                }

                //gán lại các giá trị
                var room = db.Rooms.Find(booking.RoomID);
                ViewBag.RoomDetail = room;
                return View(booking);
            }


        }

        private void SendInfoBookingToCustomer(Booking booking)
        {
            //tài khoản gửi
            string fromEmail = "artemishotel.contact@gmail.com";
            string emailPassword = "tzff yhmr weaj ctof";

            //Email nhận thông tin của khách hàng
            string customerEmail = booking.Email;
            //Tiêu đề email
            string subject = "Đơn đặt phòng của bạn từ Artemis";
            //Nội dung Email
            string body = "Thông tin đơn đặt phòng của bạn \n";
            body += "Họ tên: " + booking.CustomerName + "\n";
            body += "Loại phòng: " + booking.Room.RoomType.RoomTypeName + "\n";
            body += "Email: " + booking.Email + "\n";
            body += "Số điện thoại: " + booking.Phone + "\n";
            body += "Người lớn: " + booking.Adult + "\n";
            body += "Trẻ em: " + booking.Child + "\n";
            body += "Ngày đặt phòng: " + booking.BookingDate + "\n";
            body += "Ghi chú: " + booking.Note + "\n";

            MailMessage message = new MailMessage(fromEmail, customerEmail, subject, body);

            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);
            smtpClient.EnableSsl = true;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new NetworkCredential(fromEmail, emailPassword);
            smtpClient.Send(message);
        }
    }
}