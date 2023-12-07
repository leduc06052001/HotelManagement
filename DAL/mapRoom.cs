using DAL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class mapRoom
    {
        HMEntities db = new HMEntities();
        public List<Room> LoadData()
        {
            return db.Rooms.ToList();
        }

        public List<Room> LoadPage(int page, int size)
        {
            return db.Rooms.ToList().Skip((page - 1) * size).Take(size).ToList();
        }

        public Room GetDetail(int id)
        {
            return db.Rooms.Find(id);
        }

        //------------------* Create *------------------//
        public int CreateRoom(Room room)
        {
            if (room.RoomNumber == 0)
            {
                return 0;
            }
            db.Rooms.Add(room);
            db.SaveChanges();
            return room.RoomID;
        }

        //------------------* Update *------------------//
        public bool UpdateRoom(Room room)
        {
            var roomInfo = db.Rooms.Find(room.RoomID);
            if (roomInfo == null)
            {
                return false;
            }
            roomInfo.RoomNumber = room.RoomNumber;
            roomInfo.RoomTypeID = room.RoomTypeID;
            roomInfo.Bed = room.Bed;
            roomInfo.Bath = room.Bath;
            roomInfo.Price = room.Price;
            roomInfo.Image = room.Image;
            roomInfo.IsActive = room.IsActive;
            roomInfo.Description = room.Description;
            db.SaveChanges();
            return true;
        }

        //------------------* Delete *------------------//
        public void DeleteRoom(int id)
        {
            var roomInfo = db.Rooms.Find(id);
            db.Rooms.Remove(roomInfo);
            db.SaveChanges();
        }
    }
}
