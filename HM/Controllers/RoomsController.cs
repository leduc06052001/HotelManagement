using DAL;
using DAL.Entity;
using HM.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HM.Controllers
{
    public class RoomsController : Controller
    {
        HMEntities db = new HMEntities();
        public ActionResult Rooms()
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

        public ActionResult Select_Room()
        {
            return View();
        }
    }
}