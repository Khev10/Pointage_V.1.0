using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using pointage.Model;

namespace pointage.Controllers
{
    public class CouponsController : Controller
    {
        private EcommerceDBEntities db = new EcommerceDBEntities();

        // GET: Coupons
        public ActionResult ViewCoupon()
        {
            var coupon = db.coupon.Include(c => c.events);
            return View(coupon.ToList());
        }

        // GET: Coupons/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            coupon coupon = db.coupon.Find(id);
            if (coupon == null)
            {
                return HttpNotFound();
            }
            return View(coupon);
        }

        // GET: Coupons/Create
        public ActionResult AddCoupon()
        {
            ViewBag.event_id = new SelectList(db.events, "event_id", "event_name");
            return View();
        }

        // POST: Coupons/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddCoupon([Bind(Include = "coupon_id,event_id,coupon_name,discount_val")] coupon coupon)
        {
            
            if (ModelState.IsValid)
            {
                var query = from c in db.coupon
                            where c.coupon_name == coupon.coupon_name
                            select c;
                if (query.Any())
                {
                    //return Content("<script>alert('Coupin Name Already Exist'); history.go(-1)</script>");
                    ViewBag.CouponExist = "Coupon Name Already Exist";
                }
                else
                {
                    db.coupon.Add(coupon);
                    db.SaveChanges();
                    return RedirectToAction("ViewCoupon");
                }
         
            }

            ViewBag.event_id = new SelectList(db.events, "event_id", "event_name", coupon.event_id);
            return View(coupon);
        }

        // GET: Coupons/Edit/5
        public ActionResult EditCoupon(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            coupon coupon = db.coupon.Find(id);
            if (coupon == null)
            {
                return HttpNotFound();
            }
            ViewBag.event_id = new SelectList(db.events, "event_id", "event_name", coupon.event_id);
            return View(coupon);
        }

        // POST: Coupons/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditCoupon([Bind(Include = "coupon_id,event_id,coupon_name,discount_val")] coupon coupon)
        {
            if (ModelState.IsValid)
            {
               
                    db.Entry(coupon).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("ViewCoupon");
            }
            ViewBag.event_id = new SelectList(db.events, "event_id", "event_name", coupon.event_id);
            return View(coupon);
        }

        // GET: Coupons/Delete/5
        public ActionResult DeleteCoupon(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            coupon coupon = db.coupon.Find(id);
            if (coupon == null)
            {
                return HttpNotFound();
            }
            return View(coupon);
        }

        // POST: Coupons/Delete/5
        [HttpPost, ActionName("DeleteCoupon")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            coupon coupon = db.coupon.Find(id);
            db.coupon.Remove(coupon);
            db.SaveChanges();
            return RedirectToAction("ViewCoupon");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
