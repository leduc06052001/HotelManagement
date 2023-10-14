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
        public List<Employee> LoadData()
        {
            return db.Employees.ToList();
        }

        public List<Employee> LoadPage(int page, int size)
        {
            return db.Employees.ToList().Skip((page-1)*size).Take(size).ToList();
        }

        public Employee GetDetail(int id)
        {
            return db.Employees.Find(id);
        }

        //------------------* Create *------------------//
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

        //------------------* Update *------------------//
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

        //------------------* Delete *------------------//
        public void DeleteEmployee(int id)
        {
            var employeeInfo = db.Employees.Find(id);
            db.Employees.Remove(employeeInfo);
            db.SaveChanges();
        }
    }
}
