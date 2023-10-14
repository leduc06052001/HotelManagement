using DAL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class mapCustomer
    {
        HMEntities db = new HMEntities();
        public List<Customer> LoadData()
        {
            return db.Customers.ToList();
        }

        public List<Customer> LoadPage(int page, int size)
        {
            return db.Customers.ToList().Skip((page-1)*size).Take(size).ToList();
        }

        public Customer GetDetail(int ID)
        {
            return db.Customers.Find(ID);
        }

        //------------------* Create *------------------//
        public int AddCustomer(Customer customer)
        {
            if(customer.FullName == null)
            {
                return 0;
            }
            db.Customers.Add(customer);
            db.SaveChanges();
            return customer.CustomerID;
        }

        //------------------* Update *------------------//
        public bool UpdateCustomer(Customer customer)
        {
            var customerInfo = db.Customers.Find(customer.CustomerID);
            if(customerInfo == null)
            {
                return false;
            }
            customerInfo.FullName = customer.FullName;
            customerInfo.Address = customer.Address;
            customerInfo.Phone = customer.Phone;
            customerInfo.Email = customer.Email;
            customerInfo.Gender = customer.Gender;
            customerInfo.BirthDate = customer.BirthDate;
            customerInfo.Image = customer.Image;
            customerInfo.PromotionID = customer.PromotionID;
            db.SaveChanges();
            return true;
        }

        //------------------* Delete *------------------//
        public void DeleteCustomer(int ID)
        {
            var customerInfo = db.Customers.Find(ID);
            db.Customers.Remove(customerInfo);
            db.SaveChanges();
        }
    }
}
