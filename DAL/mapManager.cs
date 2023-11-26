using DAL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class mapManager
    {
        HMEntities db = new HMEntities();
        public Manager Login(string username, string password)
        {
            var account = db.Managers.SingleOrDefault(p => p.UserName == username && p.Password == password);
            if (account != null)
            {
                return account;
            }
            else
            {
                return null;
            }
        }

        public int Register(Manager managers)
        {
            if (managers.UserName == null)
            {
                return 0;
            }
            db.Managers.Add(managers);
            db.SaveChanges();
            return managers.ManagerID;
        }

        public int InsertForFacebook(Manager manager)
        {
            var managers = db.Managers.SingleOrDefault(p => p.UserName == manager.UserName);
            if (managers == null)
            {
                db.Managers.Add(manager);
                db.SaveChanges();
                return manager.ManagerID;
            }
            else
            {
                return managers.ManagerID;
            }
        }
    }
}
