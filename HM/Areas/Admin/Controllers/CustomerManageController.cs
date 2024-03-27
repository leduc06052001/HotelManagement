using DAL;
using DAL.Entity;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HM.Areas.Admin.Controllers
{
    public class CustomerManageController : Controller
    {
        public ActionResult AllCustomer(string fullName, string email, int page = 1, int pageSize = 10)
        {
            ViewBag.fullName = fullName;
            ViewBag.email = email;
            return View(new mapCustomer().AllCustomer(fullName, email, page, pageSize));
        }

        //------------------* CREATE *------------------//
        public ActionResult AddCustomer()
        {
            return View(new Customer());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddCustomer(Customer customer)
        {
            if (ModelState.IsValid)
            {
                var data = new mapCustomer().AddCustomer(customer);
                if (data > 0)
                {
                    return RedirectToAction("AllCustomer");
                }
            }
            return View(customer);
        }

        //------------------* UPDATE *------------------//
        public ActionResult UpdateCustomer(int ID)
        {
            return View(new mapCustomer().GetDetail(ID));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateCustomer(Customer customer, HttpPostedFileBase fileImage)
        {
            if (ModelState.IsValid)
            {
                if (fileImage != null)
                {
                    if (fileImage.ContentLength > 0)
                    {
                        //Lấy đường dẫn
                        string _path = "/Areas/Admin/assets/img/profiles/";
                        string fileName = fileImage.FileName;
                        string _root = Server.MapPath(_path + fileName);

                        // Xóa file có trùng tên
                        if (System.IO.File.Exists(_root) == true)
                        {
                            System.IO.File.Delete(_root);
                        }
                        // Lưu file
                        fileImage.SaveAs(_root);
                        customer.Image = _path + fileName;
                    }
                }

                // Xử lý cập nhật
                var customerInfo = new mapCustomer();
                if (customerInfo.UpdateCustomer(customer) == true)
                {
                    return RedirectToAction("AllCustomer");
                }
            }
            return View(customer);
        }

        //------------------* DELETE *------------------//
        public ActionResult DeleteCustomer(int ID)
        {
            var mapped = new mapCustomer();
            mapped.DeleteCustomer(ID);
            return RedirectToAction("AllCustomer");
        }
    }
}