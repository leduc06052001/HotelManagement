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
        public string message = "";
        //Read
        public List<Room> LoadData()
        {
            return (db.Rooms.ToList());
        }

        public List<Room> LoadPage(int page, int size)
        {
            var listRoom = db.Rooms.ToList().Skip((page-1)*size).Take(size).ToList();
            return listRoom;
        }

        //Lấy đối tượng theo ID
        public Room GetDetail(int ID)
        { 
            return db.Rooms.Find(ID);
        }

        //Create
        //Lưu phòng mới được thêm vào và trả về ID phòng
        public int AddRoom(Room room)
        {
            if(room.RoomNumber == null)
            {
                message = "Thiếu thông tin số phòng";
                return 0;
            }
            db.Rooms.Add(room);
            db.SaveChanges();
            return room.RoomID;
        }

        //Update
        public bool EditRoom(Room room)
        {
            var roomInfo = db.Rooms.Find(room.RoomID);
            if(roomInfo == null)
            {
                message = "Không tìm thấy thông tin phòng";
                return false;
            }
            roomInfo.RoomNumber = room.RoomNumber;
            roomInfo.RoomTypeID = room.RoomTypeID;
            roomInfo.Bed = room.Bed;
            roomInfo.Bath = room.Bath;
            roomInfo.Price = room.Price;
            roomInfo.Image = room.Image;
            roomInfo.Description = room.Description;
            db.SaveChanges();
            return true;
        }
        //Delete

        public void DeleteRoom(int ID)
        {
            var roomInfo = db.Rooms.Find(ID);
            db.Rooms.Remove(roomInfo);
            db.SaveChanges();
        }
    }
}
