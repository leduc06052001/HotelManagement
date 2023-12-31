﻿using DAL;
using DAL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HM.Areas.Admin.Controllers
{
    public class CustomerManageController : Controller
    {
        public ActionResult AllCustomer()
        { 
            return View(new mapCustomer().LoadData());
        }

        //------------------* CREATE *------------------//
        public ActionResult AddCustomer()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddCustomer(Customer customer)
        {
            var data = new mapCustomer().AddCustomer(customer);
            if(data > 0)
            {
                return RedirectToAction("AllCustomer");
            }
            else
            {
                return View(customer);
            }
        }

        //------------------* UPDATE *------------------//
        public ActionResult UpdateCustomer(int ID)
        {
            return View(new mapCustomer().GetDetail(ID));
        }

        [HttpPost]
        public ActionResult UpdateCustomer(Customer customer, HttpPostedFileBase fileImage)
        {
            if(fileImage != null)
            {
                if(fileImage.ContentLength > 0)
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
            var mapped = new mapCustomer();
            if(mapped.UpdateCustomer(customer) == true)
            {
                return RedirectToAction("AllCustomer");
            }
            else
            {
                return View(customer);
            }
        }

        //------------------* DELETE *------------------//
        public ActionResult DeleteCustomer(int ID)
        {
            var mapped = new mapCustomer();
            mapped.DeleteCustomer(ID);
            return RedirectToAction("AllRoom");
        }
    }
}