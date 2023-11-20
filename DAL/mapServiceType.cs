using DAL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class mapServiceType
    {
        HMEntities db = new HMEntities();
        public List<ServiceType> LoadData()
        {
            return db.ServiceTypes.ToList();
        }
    }
}
