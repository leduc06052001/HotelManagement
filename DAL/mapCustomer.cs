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

        public Customer Login(string email, string password)
        {
            var account = db.Customers.SingleOrDefault(p => p.Email == email & p.Password == password);
            if (account != null)
            {
                return account;
            }
            else
            {
                return null;
            }
        }

        public int InsertForFacebook(Customer customer)
        {
            var customerInfo = db.Customers.SingleOrDefault(p => p.FullName == customer.FullName);
            if (customerInfo == null)
            {
                db.Customers.Add(customer);
                db.SaveChanges();
                return customer.CustomerID;
            }
            else
            {
                return customerInfo.CustomerID;
            }
        }

        public List<Customer> LoadData()
        {
            return db.Customers.ToList();
        }

        public List<Customer> LoadPage(string fullName, string email, int page, int size)
        {
            IQueryable<Customer> data = db.Customers;
            if (!string.IsNullOrEmpty(fullName))
            {
                data = data.Where(p => p.FullName.ToLower().Contains(fullName));
            }
            if (!string.IsNullOrEmpty(email))
            {
                data = data.Where(p => p.Email.ToLower().Contains(email));
            }
            return data.OrderBy(p => p.FullName).Skip((page - 1) * size).Take(size).ToList();
        }

        public List<Customer> Filter(string fullName, string email)
        {
            var result = db.Customers.Where(p => p.FullName.ToLower().Contains(fullName) || p.Email.ToLower().Contains(email)).ToList();
            return result;
        }

        public Customer GetDetail(int ID)
        {
            return db.Customers.Find(ID);
        }

        //------------------* Create *------------------//
        public int AddCustomer(Customer customer)
        {
            if (customer.FullName == null)
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
            if (customerInfo == null)
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
