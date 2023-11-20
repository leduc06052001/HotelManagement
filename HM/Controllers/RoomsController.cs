using DAL;
using DAL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QL_khachSan.Controllers
{
    public class RoomsController : Controller
    {
        HMEntities db = new HMEntities();
        public ActionResult Rooms()
        {
            return View();
        }

        public ActionResult Booking()
        {
            return View();
        }

        public ActionResult RoomList()
        {
            return View();
        }

        public ActionResult RoomDetail(int ID)
        {
            var rooms = db.Rooms.Find(ID);
            if (rooms == null)
            {
                return HttpNotFound();
            }
            return View(rooms);
        }

        [HttpPost]
        public ActionResult RoomDetail(OrderDetail order)
        {
            if(new mapOrder().AddOrder(order) > 0)
            {
                return RedirectToAction("Booking");
            }
            return View(order);
        }

        public ActionResult Select_Room()
        {
            return View();
        }
    }
}