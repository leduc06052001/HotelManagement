using DAL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class mapEmployee
    {
        HMEntities db = new HMEntities();

        //------------------* ALL EMPLOYEES & SEARCH(Load Page) *------------------//
        public List<Employee> AllEmployees(string employeeName, string position, int page, int size)
        {
            IQueryable<Employee> data = db.Employees;
            if (!string.IsNullOrEmpty(employeeName))
            {
                data = db.Employees.Where(p => p.FullName.ToLower().Contains(employeeName));
            }
            if(!string.IsNullOrEmpty(position))
            {
                data = db.Employees.Where(p=>p.Position.PositionName.ToLower().Contains(position));
            }
            return data.OrderBy(p=>p.EmployeeID).Skip((page-1)*size).Take(size).ToList();
        }

        //------------------* DETAIL EMPLOYEE *------------------//
        public Employee GetDetail(int id)
        {
            return db.Employees.Find(id);
        }

        //------------------* CREATE *------------------//
        public int CreateEmployee(Employee employee)
        {
            if(employee.FullName == null)
            {
                return 0;
            }
            db.Employees.Add(employee);
            db.SaveChanges();
            return employee.EmployeeID;
        }

        //------------------* UPDATE *------------------//
        public bool UpdateEmployee(Employee employee)
        {
            var employeeInfo = db.Employees.Find(employee.EmployeeID);
            if(employeeInfo != null)
            {
                employeeInfo.FullName = employee.FullName;
                employeeInfo.Address = employee.Address;
                employeeInfo.Phone = employee.Phone;
                employeeInfo.Email = employee.Email;
                employeeInfo.Gender = employee.Gender;
                employeeInfo.BirthDate = employee.BirthDate;
                employeeInfo.PositionID = employee.PositionID;
                db.SaveChanges();
                return true;
            }
            return false;
        }

        //------------------* DELETE *------------------//
        public void DeleteEmployee(int id)
        {
            var employeeInfo = db.Employees.Find(id);
            db.Employees.Remove(employeeInfo);
            db.SaveChanges();
        }
    }
}
