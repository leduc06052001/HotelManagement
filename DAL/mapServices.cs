using DAL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class mapServices
    {
        HMEntities db = new HMEntities();
        public List<Service> LoadData()
        {
            return db.Services.ToList();
        }

        public Service GetDetail(int id)
        {
            return db.Services.Find(id);
        }

        //------------------* Create *------------------//
        public int AddServices(Service services)
        {
            if(services.ServiceName == null)
            {
                return 0;
            }
            db.Services.Add(services);
            db.SaveChanges();
            return services.ServiceID;
        }

        //------------------* Update *------------------//
        public bool UpdateServices(Service services)
        {
            var serviceInfo = db.Services.Find(services.ServiceID);
            if (serviceInfo == null)
            {
                return false;
            }
            serviceInfo.ServiceName = services.ServiceName;
            serviceInfo.Price = services.Price;
            serviceInfo.Description = services.Description;
            serviceInfo.ServiceTypeID = services.ServiceTypeID;
            serviceInfo.Image = services.Image;
            db.SaveChanges();
            return true;
        }

        //------------------* Delete *------------------//
        public void DeleteServices(int id)
        {
            var serviceInfo = db.Services.Find(id);
            db.Services.Remove(serviceInfo);
            db.SaveChanges();
        }
    }
}
