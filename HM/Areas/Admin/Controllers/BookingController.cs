using DAL;
using DAL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HM.Areas.Admin.Controllers
{
    public class BookingController : Controller
    {
        public ActionResult AllBooking()
        {
            return View();
        }

        //------------------* ADD BOOKING *------------------//
        public ActionResult AddBooking()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddBooking(Booking booking)
        {
            
            if(new mapBookings().AddBooking(booking) > 0)
            {
                return RedirectToAction("AllBooking");
            }
            return View(booking);
        }

        //------------------* EDIT BOOKING *------------------//
        public ActionResult EditBooking()
        {
            return View();
        }
    }
}