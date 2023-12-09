using DAL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.ModelBinding;

namespace DAL
{
    public class mapCustomer
    {
        HMEntities db = new HMEntities();

        //------------------* LOGIN *------------------//
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

        //------------------* ADD ACCOUNT LOGIN WITH FB *------------------//
        public int InsertForFacebook(Customer customer)
        {
            var customerInfo = db.Customers.SingleOrDefault(p => p.Email == customer.Email);
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

        //------------------* ALL CUSTOMERS & SEARCH(Load page) *------------------//
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
            return data.OrderBy(p => p.CustomerID).Skip((page - 1) * size).Take(size).ToList();
        }

        //------------------* GET DETAIL CUSTOMER *------------------//
        public Customer GetDetail(int ID)
        {
            return db.Customers.Find(ID);
        }

        //------------------* CREATE *------------------//
        public int AddCustomer(Customer customer)
        {
            if (customer == null)
            {
                return 0;
            }
            db.Customers.Add(customer);
            db.SaveChanges();
            return customer.CustomerID;
        }

        //------------------* UPDATE *------------------//
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

        //------------------* DELETE *------------------//
        public void DeleteCustomer(int ID)
        {
            var customerInfo = db.Customers.Find(ID);
            db.Customers.Remove(customerInfo);
            db.SaveChanges();
        }
    }
}
