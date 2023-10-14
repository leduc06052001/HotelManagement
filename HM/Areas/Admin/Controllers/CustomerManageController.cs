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
            mapCustomer mapped = new mapCustomer();
            return View(mapped.LoadData());
        }

        //--- Create Customer ---
        public ActionResult AddCustomer()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddCustomer(Customer customer)
        {
            mapCustomer mapped = new mapCustomer();
            var data = mapped.AddCustomer(customer);
            if(data > 0)
            {
                return RedirectToAction("AllCustomer");
            }
            else
            {
                return View(customer);
            }
        }


        //--- Update Customer ---
        public ActionResult UpdateCustomer(int ID)
        {
            var customerInfo = new mapCustomer().GetDetail(ID);
            return View(customerInfo);
        }

        [HttpPost]
        public ActionResult UpdateCustomer(Customer customer, HttpPostedFileBase fileImage)
        {
            if(fileImage != null)
            {
                if(fileImage.ContentLength > 0)
                {
                    string _path = "/Areas/Admin/assets/img/profiles/";
                    string fileName = fileImage.FileName;
                    string _root = Server.MapPath(_path + fileName);
                    if (System.IO.File.Exists(_root) == true)
                    {
                        System.IO.File.Delete(_root);
                    }
                    //5. Lưu file
                    fileImage.SaveAs(_root);
                    customer.Image = _path + fileName;
                }
            }
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

        //--- Delete Customer ---
        public ActionResult DeleteCustomer(int ID)
        {
            var mapped = new mapCustomer();
            mapped.DeleteCustomer(ID);
            return RedirectToAction("AllRoom");
        }
    }
}