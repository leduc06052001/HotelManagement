using DAL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class mapOrder
    {
        private HMEntities db = new HMEntities();
        string messageError = "";

        public List<OrderDetail> LoadData()
        {
            return db.OrderDetails.ToList();
        }
        public int AddOrder(OrderDetail order)
        {
            if(order == null)
            {
                messageError = "Thiếu thông tin đặt phòng";
                return 0;
            }
            db.OrderDetails.Add(order);
            db.SaveChanges();
            return order.OrderID;
        }
    }
}
