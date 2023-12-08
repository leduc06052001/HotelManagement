using DAL;
using DAL.Entity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HM.Areas.Admin.Controllers
{
    public class RoomManageController : Controller
    {
        //------------------* ALL ROOMS & SEARCH *------------------//
        public ActionResult AllRoom(string roomType, int page = 1, int size = 1001)
        {
            ViewBag.roomType = roomType;
            return View(new mapRoom().AllRooms(roomType, page, size));
        }

        //------------------* CREATE *------------------//
        public ActionResult AddRoom()
        {
            return View(new Room() { Bed = 0, Bath = 1 });
        }

        [HttpPost]
        public ActionResult AddRoom(Room room)
        {

            if (new mapRoom().CreateRoom(room) > 0)
            {
                return RedirectToAction("AllRoom");
            }
            else
            {
                return View(room);
            }
        }

        //------------------* UPDATE *------------------//
        public ActionResult EditRoom(int ID)
        {
            var roomInfo = new mapRoom().GetDetail(ID);
            return View(roomInfo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditRoom(Room room, HttpPostedFileBase fileImage)
        {
            //Xử lý file ảnh
            //Kiểm tra có tồn tại file? (người dùng có post file?) null, length
            if (fileImage != null)
            {
                if (fileImage.ContentLength > 0)
                {
                    //1. Xác định thư mục lưu
                    string _path = "/Areas/Admin/Data/";
                    //2. Xác định tên file
                    string fileName = fileImage.FileName;
                    //3. Xác định đường dẫn tuyệt đối 
                    string _root = Server.MapPath(_path + fileName);
                    //* Kiểm tra file đã tồn tại? nếu có thì xóa file cũ (ktra trùng tên?)
                    if (System.IO.File.Exists(_root) == true)
                    {
                        System.IO.File.Delete(_root);
                    }
                    //5. Lưu file
                    fileImage.SaveAs(_root);
                    room.Image = _path + fileName;
                }
            }

            var mapped = new mapRoom();
            if (mapped.UpdateRoom(room) == true)
            {
                return RedirectToAction("AllRoom");
            }
            else
            {
                return View(room);
            }
        }

        //------------------* DELETE *------------------//
        
        public ActionResult DeleteRoom(int ID)
        {
            new mapRoom().DeleteRoom(ID);
            return RedirectToAction("AllRoom");
        }
    }
}