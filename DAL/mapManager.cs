using DAL.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Common.CommandTrees;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class mapManager
    {
        HMEntities db = new HMEntities();

        //------------------* LOGIN *------------------//
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

        //------------------* REGISTER *------------------//
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

        //------------------* FORGOT PASSWORD *------------------//


        public Manager LoadData(int ID)
        {
            return db.Managers.Find(ID);
        }
    }
}
