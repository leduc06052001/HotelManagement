using DAL.Entity;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HM.Areas.Admin.Controllers
{
    public class PromotionController : Controller
    {
        //------------------* ALL ROOMS & SEARCH *------------------//
        public ActionResult AllPromotion()
        {
            return View(new mapPromotion().AllPromotion());
        }

        //------------------* CREATE *------------------//
        public ActionResult AddPromotion()
        {
            return View(new Promotion());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddPromotion(Promotion promotion)
        {
            if (ModelState.IsValid)
            {
                if (new mapPromotion().CreatePromotion(promotion) > 0)
                {
                    return RedirectToAction("AllPromotion");
                }
            }
            return View(promotion);
        }

        //------------------* UPDATE *------------------//
        public ActionResult EditPromotion(int ID)
        {
            var promotionInfo = new mapPromotion().GetDetail(ID);
            return View(promotionInfo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditPromotion(Promotion promotion)
        {
            if (ModelState.IsValid)
            {
                var mapped = new mapPromotion();
                if (mapped.UpdatePromotion(promotion) == true)
                {
                    return RedirectToAction("AllPromotion") ;
                }
            }
            return View(promotion);
        }

        //------------------* DELETE *------------------//

        public ActionResult DeletePromotion(int ID)
        {
            new mapPromotion().DeletePromotion(ID);
            return RedirectToAction("AllPromotion");
        }
    }
}