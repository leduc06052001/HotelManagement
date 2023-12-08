using DAL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class mapPosition
    {
        HMEntities db = new HMEntities();
        public List<Position> LoadData()
        {
            return db.Positions.ToList();
        }
    }
}
