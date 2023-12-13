using DAL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HM.App_Start
{
    public static class SessionConfig
    {
        //Lưu session
        public static void SetUser(Manager manager)
        {
            HttpContext.Current.Session["manager"] = manager;
        }
        
        //Lấy session
        public static Manager GetUser()
        {
            return (Manager)HttpContext.Current.Session["manager"];
        }
    }

    public static class UserSessionConfig
    {
        public static void SetCustomer(Customer customer)
        {
            HttpContext.Current.Session["customer"] = customer;
        }
        public static Customer GetCustomer()
        {
            return (Customer)HttpContext.Current.Session["customer"];
        }
    }
}