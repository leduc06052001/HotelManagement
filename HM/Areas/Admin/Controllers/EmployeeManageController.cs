using Antlr.Runtime.Tree;
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

        //------------------* Create *------------------//
        public ActionResult AddEmployee()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddEmployee(Employee employee)
        {
            var mapped = new mapEmployee();
            if(mapped.CreateEmployee(employee) > 0) 
            {
                return RedirectToAction("AllEmployee");
            }
            else
            {
                return View(employee);
            }
        }

        //------------------* Update *------------------//
        public ActionResult UpdateEmployee(int ID)
        {
            var employeeInfo = new mapEmployee().GetDetail(ID);
            return View(employeeInfo);
        }
        [HttpPost]
        public ActionResult UpdateEmployee(Employee employee)
        {
            var mapped = new mapEmployee();
            if(mapped.UpdateEmployee(employee) == true)
            {
                return RedirectToAction("AllEmployee");
            }
            else
            {
                return View(employee);
            }
        }

        //------------------* Delete *------------------//
        public ActionResult DeleteEmployee(int ID)
        {
            var mapped = new mapEmployee();
            mapped.DeleteEmployee(ID);
            return RedirectToAction("AllEmployee");
        }
    }
}