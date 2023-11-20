using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DAL;

namespace HM.Areas.Admin.Controllers
{
    public class RoomTypeController : Controller
    {
        public ActionResult RoomType()
        {
            return View(new mapRoomType().LoadData());
        }
    }
}