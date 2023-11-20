using DAL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class mapBookings
    {
        private HMEntities db = new HMEntities();
        public int AddBooking(Booking booking)
        {
            if(booking == null)
            {
                return 0;
            }
            db.Bookings.Add(booking);
            db.SaveChanges();
            return booking.BookingID;
        }
    }
}
