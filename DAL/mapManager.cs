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
    }
}
