using DAL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HM.Common
{
    public interface IRoomService
    {
        void UpdateRoomStatus(int roomID, string newStatus);
        void ActivateRoom(int roomID);
    }
    public class RoomService : IRoomService
    {
        public void UpdateRoomStatus(int roomID, string newStatus)
        {

        }
        public void ActivateRoom(int roomID)
        {
            
        }
    }
}