using DAL;
using DAL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HM.Areas.Admin.Controllers
{
    public class EmployeeManageController : Controller
    {
        public ActionResult AllEmployee()
        {
            var mapped = new mapEmployee();
            return View(mapped.LoadData());
        }

        public ActionResult AddEmployee()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddEmployee(Employee employee)
        {
            var mapped = new mapEmployee();
            if(mapped.AddEmployee(employee) > 0) 
            {
                return RedirectToAction("AllEmployee");
            }
            else
            {
                return View(employee);
            }
        }

        public ActionResult EditEmployee()
        {
            return View();
        }
    }
}