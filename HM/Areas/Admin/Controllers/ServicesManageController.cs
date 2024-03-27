using DAL;
using DAL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HM.Areas.Admin.Controllers
{
    public class ServicesManageController : Controller
    {
        public ActionResult AllServices()
        {
            return View(new mapServiceType().LoadData());
        }

        //------------------* CREATE *------------------//
        public ActionResult AddServices()
        {
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult AddServices(Service services)
        {
            var data = new mapServices().AddServices(services);
            if(data > 0)
            {
                return RedirectToAction("AllServices");
            }
            else
            {
                return View(services);
            }
        }

        //------------------* UPDATE *------------------//
        public ActionResult UpdateServices(int id)
        {
            return View(new mapServices().GetDetail(id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult UpdateServices(Service services, HttpPostedFileBase fileImage)
        {
            //Xử lý file ảnh
            //Kiểm tra dữ liệu đầu vào?
            if (fileImage != null)
            {
                if (fileImage.ContentLength > 0)
                {
                    //Thư mục lưu
                    var _path = "/Areas/Admin/Data/Service_Image/";
                    //Tên file
                    var fileName = fileImage.FileName;
                    //Đường dãn
                    var _root = Server.MapPath(_path + fileName);
                    //ktra tồn tại
                    if (System.IO.File.Exists(_root))
                    {
                        System.IO.File.Delete(_root);
                    }
                    //Lưu file
                    fileImage.SaveAs(_root);
                    services.Image = _path + fileName;
                }
            }
            //Xử lý cập nhật
            var data = new mapServices().UpdateServices(services);
            if(data == true)
            {
                return RedirectToAction("AllServices");
            }
            return View(services);
        }

        //------------------* DELETE *------------------//
        public ActionResult DeleteServices(int id)
        {
            new mapServices().DeleteServices(id);
            return RedirectToAction("AllService");
        }
    }
}