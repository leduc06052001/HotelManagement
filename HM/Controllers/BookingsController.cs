using Antlr.Runtime.Tree;
using DAL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
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
    }
}