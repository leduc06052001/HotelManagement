using DAL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HM.Common
{
    public static class SessionUser
    {
        public static void SetUser(Customer customer)
        {
            HttpContext.Current.Session["user"] = customer;
        }

        public static Customer GetUser()
        {
            return (Customer)HttpContext.Current.Session["user"];
        }
    }
}