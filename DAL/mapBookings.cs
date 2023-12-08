using DAL.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.ModelBinding;

namespace DAL
{
    public class mapBookings
    {
        private HMEntities db = new HMEntities();

        //------------------* All Rooms & Search(load page) *------------------//
        public List<Booking> AllBookings(string customerName, string email, int page, int size)
        {
            IQueryable<Booking> data = db.Bookings;
            if (!string.IsNullOrEmpty(customerName))
            {
                data = data.Where(p => p.CustomerName.ToLower().Contains(customerName));
            }
            if (!string.IsNullOrEmpty(email))
            {
                data = data.Where(p => p.Email.ToLower().Contains(email));
            }
            return data.OrderBy(p => p.CustomerName).Skip((page - 1) * size).Take(size).ToList();
        }

        //------------------* Get Detail *------------------//
        public Booking GetDetail(int ID)
        {
            return db.Bookings.Find(ID);
        }

        //------------------* CREATE *------------------//
        public int AddBooking(Booking booking)
        {
            if (booking == null)
            {
                return 0;
            }
            if(booking.CheckinDate.HasValue && booking.CheckoutDate.HasValue)
            {
                TimeSpan timeSpan = booking.CheckoutDate.Value - booking.CheckinDate.Value;
                booking.TotalAmount = (int)timeSpan.TotalDays * booking.Room.Price; 
            }
            db.Bookings.Add(booking);
            db.SaveChanges();
            return booking.BookingID;
        }

        //------------------* UPDATE *------------------//
        public bool EditBooking(Booking booking)
        {
            var bookingInfo = db.Bookings.Find(booking.BookingID);
            if(bookingInfo == null)
            {
                return false;
            }
            bookingInfo.CustomerName = booking.CustomerName;
            bookingInfo.RoomID = booking.RoomID;
            bookingInfo.Adult = booking.Adult;
            bookingInfo.Child = booking.Child;
            bookingInfo.BookingDate = booking.BookingDate;
            bookingInfo.CheckinDate = booking.CheckinDate;
            bookingInfo.CheckoutDate = booking.CheckoutDate;
            bookingInfo.Email = booking.Email;
            bookingInfo.Phone= booking.Phone;
            bookingInfo.Note = booking.Note;
            db.SaveChanges();
            return true;
        }

        //------------------* DELETE *------------------//
        public void DeleteBooking(int ID)
        {
            var bookingInfo = db.Bookings.Find(ID);
            db.Bookings.Remove(bookingInfo);
            db.SaveChanges();
        }
    }
}
