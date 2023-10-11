using DAL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class mapPromotion
    {
        HMEntities db = new HMEntities();
        public List<Promotion> LoadData()
        {
            return db.Promotions.ToList();
        }
    }
}
