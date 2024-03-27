using Antlr.Runtime.Tree;
using DAL;
using DAL.Entity;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HM.Areas.Admin.Controllers
{
    public class EmployeeManageController : Controller
    {
        public ActionResult AllEmployees(string employeeName, string position, int page = 1, int size = 1001)
        {
            ViewBag.employeeName = employeeName;
            ViewBag.position = position;
            return View(new mapEmployee().AllEmployees(employeeName, position, page, size));
        }

        //------------------* CREATE *------------------//
        public ActionResult AddEmployee()
        {
            
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddEmployee(Employee employee)
        {
            if (ModelState.IsValid)
            {
                if (new mapEmployee().CreateEmployee(employee) > 0)
                {
                    return RedirectToAction("AllEmployees");
                }
                else
                {
                    return View(employee);
                }
            }
            
            return View(employee);

        }

        //------------------* UPDATE *------------------//
        public ActionResult UpdateEmployee(int ID)
        {
            return View(new mapEmployee().GetDetail(ID));
        }
        [HttpPost]
        public ActionResult UpdateEmployee(Employee employee)
        {
            if (new mapEmployee().UpdateEmployee(employee) == true)
            {
                return RedirectToAction("AllEmployees");
            }
            else
            {
                return View(employee);
            }
        }

        //------------------* DELETE *------------------//
        public ActionResult DeleteEmployee(int ID)
        {
            new mapEmployee().DeleteEmployee(ID);
            return RedirectToAction("AllEmployees");
        }
    }
}