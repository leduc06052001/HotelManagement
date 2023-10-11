using DAL.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class mapEmployee
    {
        HMEntities db = new HMEntities();
        public string message = "";
        public List<Employee> LoadData()
        {
            return db.Employees.ToList();
        }

        // Load page
        public List<Employee> LoadPage(int page, int size)
        {
            return db.Employees.ToList().Skip((page-1)*size).Take(size).ToList();
        }

        // Get detail (object)
        public Employee GetDetail(int ID)
        {
            return db.Employees.Find(ID);
        }

        // Create
        public int AddEmployee(Employee employee)
        {
            if(employee.FullName == null)
            {
                message = "Employee name is not valid";
                return 0;
            }
            db.Employees.Add(employee);
            db.SaveChanges();
            return employee.EmployeeID;
        }
    }
}
