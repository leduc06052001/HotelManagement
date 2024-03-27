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
        public List<Promotion> AllPromotion()
        {
            return db.Promotions.ToList();
        }
        //------------------* GET ROOM DETAIL *------------------//
        public Promotion GetDetail(int id)
        {
            return db.Promotions.Find(id);
        }

        //------------------* CREATE *------------------//
        public int CreatePromotion(Promotion promotion)
        {
            if (promotion == null)
            {
                return 0;
            }
            db.Promotions.Add(promotion);
            db.SaveChanges();
            return promotion.PromotionID;
        }

        //------------------* UPDATE *------------------//
        public bool UpdatePromotion(Promotion promotion)
        {
             var promotiontInfo = db.Promotions.Find(promotion.PromotionID);
            if (promotiontInfo == null)
            {
                return false;
            }
            promotiontInfo.PromotionName = promotion.PromotionName;
            promotiontInfo.StartDate = promotion.StartDate;
            promotiontInfo.EndDate = promotion.EndDate;
            promotiontInfo.PromotinValues = promotion.PromotinValues;
            db.SaveChanges();
            return true;
        }

        //------------------* DELETE *------------------//
        public void DeletePromotion(int id)
        {
            var promotionInfo = db.Promotions.Find(id);
            db.Promotions.Remove(promotionInfo);
            db.SaveChanges();
        }
    }
}
