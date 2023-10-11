using DAL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class mapRoomType
    {
        HMEntities db = new HMEntities();
        public List<RoomType> LoadData()
        {
            return db.RoomTypes.ToList();
        }
    }
}
