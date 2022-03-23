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
    public class CheckoutsController : Controller
    {
        private EcommerceDBEntities db = new EcommerceDBEntities();

        // GET: Checkouts
        public ActionResult Checkout(int? id)
        {
            cart Carts = db.cart.Where(c => c.cart_id == id).FirstOrDefault();
            return View(Carts);
        }

        [HttpPost]
        public ActionResult PlaceOrder(checkout Check)
        {
            Check.cart_id = int.Parse(Session["cart_id"].ToString());
            if (Session["couponid"] != null)
            {
                Check.coupon_id = int.Parse(Session["couponid"].ToString());
            }
            else
            {
                Check.coupon_id = null;
            }
            Check.total_price = decimal.Parse(Session["totalprice"].ToString());
            Check.checkout_date = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
            Check.delivery_date = Convert.ToDateTime(DateTime.Now.Date.AddDays(8).ToString("yyyy-MM-dd"));
            Check.payment_method = "COD";
            db.checkout.Add(Check);
            db.SaveChanges();
            updatestocks();
            deleteCarts();
            Session["coupon"] = null;
            return RedirectToAction("Cart", "CartItems");
        }

        private void deleteCarts()
        {
            int cartid = int.Parse(Session["cart_id"].ToString());
            var deleteCart = db.cart_items.Where(c => c.cart_id == cartid);
            db.cart_items.RemoveRange(deleteCart);
            db.SaveChanges();
        }

        public void updatestocks()
        {
            product Products = new product();
            int cartid = int.Parse(Session["cart_id"].ToString());
            var cart_items = db.cart_items.Include(c => c.cart).Include(c => c.product).Include(c => c.coupon);
            cart_items = cart_items.Where(c => c.cart_id == cartid);
            foreach (var item in cart_items.ToList())
            {
                var productitem = db.product.Find(item.product_id);
                productitem.product_available_qty -= item.product_qty;
                db.Entry(productitem).State = EntityState.Modified;
                db.SaveChanges();
            }
        }

        // GET: Checkouts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            checkout checkout = db.checkout.Find(id);
            if (checkout == null)
            {
                return HttpNotFound();
            }
            return View(checkout);
        }

        // GET: Checkouts/Create
        public ActionResult Create()
        {
            ViewBag.cart_id = new SelectList(db.cart, "cart_id", "cart_id");
            ViewBag.coupon_id = new SelectList(db.coupon, "coupon_id", "coupon_name");
            return View();
        }

        // POST: Checkouts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "checkout_id,cart_id,coupon_id,total_price,checkout_date,delivery_date,payment_method")] checkout checkout)
        {
            if (ModelState.IsValid)
            {
                db.checkout.Add(checkout);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.cart_id = new SelectList(db.cart, "cart_id", "cart_id", checkout.cart_id);
            ViewBag.coupon_id = new SelectList(db.coupon, "coupon_id", "coupon_name", checkout.coupon_id);
            return View(checkout);
        }

        // GET: Checkouts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            checkout checkout = db.checkout.Find(id);
            if (checkout == null)
            {
                return HttpNotFound();
            }
            ViewBag.cart_id = new SelectList(db.cart, "cart_id", "cart_id", checkout.cart_id);
            ViewBag.coupon_id = new SelectList(db.coupon, "coupon_id", "coupon_name", checkout.coupon_id);
            return View(checkout);
        }

        // POST: Checkouts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "checkout_id,cart_id,coupon_id,total_price,checkout_date,delivery_date,payment_method")] checkout checkout)
        {
            if (ModelState.IsValid)
            {
                db.Entry(checkout).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.cart_id = new SelectList(db.cart, "cart_id", "cart_id", checkout.cart_id);
            ViewBag.coupon_id = new SelectList(db.coupon, "coupon_id", "coupon_name", checkout.coupon_id);
            return View(checkout);
        }

        // GET: Checkouts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            checkout checkout = db.checkout.Find(id);
            if (checkout == null)
            {
                return HttpNotFound();
            }
            return View(checkout);
        }

        // POST: Checkouts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            checkout checkout = db.checkout.Find(id);
            db.checkout.Remove(checkout);
            db.SaveChanges();
            return RedirectToAction("Index");
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
