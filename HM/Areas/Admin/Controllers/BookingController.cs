using DAL;
using DAL.Entity;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.UI;

namespace HM.Areas.Admin.Controllers
{
    public class BookingController : Controller
    {
        public ActionResult AllBooking(string customerName, string email,string roomType, int page = 1, int size = 1001)
        {
            ViewBag.customerName = customerName;
            ViewBag.email = email;
            ViewBag.roomType = roomType;
            return View(new mapBookings().AllBookings(customerName, email, roomType, page, size));
        }

        //------------------* ADD BOOKING *------------------//
        public ActionResult AddBooking()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddBooking(Booking booking)
        {
            if (ModelState.IsValid)
            {
                if (new mapBookings().AddBooking(booking) > 0)
                {
                    return RedirectToAction("AllBooking");
                }
            }
            return View(booking);
        }

        //------------------* EDIT BOOKING *------------------//
        public ActionResult EditBooking(int ID)
        {
            return View(new mapBookings().GetDetail(ID));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditBooking(Booking booking)
        {
            if (ModelState.IsValid)
            {
                var bookingInfo = new mapBookings().EditBooking(booking);
                if (bookingInfo == true)
                {
                    return RedirectToAction("AllBooking");
                }
            }
            return View(booking);
        }

        public ActionResult DeleteBooking(int ID)
        {
            new mapBookings().DeleteBooking(ID);
            return RedirectToAction("AllBooking");
        }
    }
}