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

        //------------------* ALL ROOMS & SEARCH(Load Page) *------------------//
        public List<Room> AllRooms(string roomType, int page, int size)
        {
            IQueryable<Room> data = db.Rooms;
            /*if(roomNo > 0)
            {
                data = db.Rooms.Where(p => p.RoomNumber == roomNo);
            }*/
            if(!string.IsNullOrEmpty(roomType))
            {
                data = db.Rooms.Where(p => p.RoomType.RoomTypeName.ToLower().Contains(roomType));
            }
            return data.OrderBy(p=>p.RoomID).Skip((page-1)*size).Take(size).ToList();
        }

        public List<Room> LoadData()
        {
            return db.Rooms.ToList();
        }

        //------------------* GET ROOM DETAIL *------------------//
        public Room GetDetail(int id)
        {
            return db.Rooms.Find(id);
        }

        //------------------* CREATE *------------------//
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

        //------------------* UPDATE *------------------//
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

        //------------------* DELETE *------------------//
        public void DeleteRoom(int id)
        {
            var roomInfo = db.Rooms.Find(id);
            db.Rooms.Remove(roomInfo);
            db.SaveChanges();
        }
    }
}
