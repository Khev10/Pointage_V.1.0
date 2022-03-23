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
    public class CartItemsController : Controller
    {
        private EcommerceDBEntities db = new EcommerceDBEntities();

        // GET: CartItems
        public ActionResult Cart()
        {
            if (Session["cart_id"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            var cart_items = db.cart_items.Include(c => c.cart).Include(c => c.product).Include(c => c.coupon);
            int cartid = int.Parse(Session["cart_id"].ToString());
            var cartItems = db.cart_items.Where(x => x.cart_id == cartid).Count();
            Session["cartcounter"] = cartItems;
            cart_items = cart_items.Where(c => c.cart_id == cartid);
            if (Session["coupon"] != null)
            {
                Session["totalprice"] = decimal.Parse(Session["totalprice"].ToString()) - decimal.Parse(Session["coupondiscount"].ToString());

            }
            else
            {
                Session["totalprice"] = cart_items.Sum(c => c.product_qty * c.product.product_price);
            }


            return View(cart_items.ToList());
        }

        public ActionResult OrderNow(int? id)
        {
            cart_items cart_items = new cart_items();
            int cartid = int.Parse(Session["cart_id"].ToString());
            var check_cart_items = db.cart_items.Where(c => c.product_id == id && c.cart_id == cartid).FirstOrDefault();
            if (check_cart_items != null)
            {
                check_cart_items.product_qty += 1;
                db.Entry(check_cart_items).State = EntityState.Modified;
            }
            else if (check_cart_items == null)
            {
                cart_items.cart_id = int.Parse(Session["cart_id"].ToString());
                cart_items.product_id = id;
                cart_items.product_qty = 1;
                db.cart_items.Add(cart_items);
            }

            db.SaveChanges();
            return RedirectToAction("Cart");
        }
        public ActionResult add_item(int? id)
        {
            cart_items cart_items = db.cart_items.Find(id);
            cart_items.product_qty++;
            db.Entry(cart_items).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Cart");
        }
        public ActionResult minus_item(int? id)
        {
            cart_items cart_items = db.cart_items.Find(id);
            cart_items.product_qty--;
            db.Entry(cart_items).State = EntityState.Modified;
            db.SaveChanges();
            if (cart_items.product_qty == 0)
            {
                Delete(id);
            }
            return RedirectToAction("Cart");
        }
        public ActionResult Delete(int? id)
        {
            cart_items cart_items = db.cart_items.Find(id);
            db.cart_items.Remove(cart_items);
            db.SaveChanges();
            return RedirectToAction("Cart");
        }
        [HttpPost]
        public ActionResult AddCoupon(coupon Coupons)
        {
            var coupon = db.coupon.Where(c => c.coupon_name.Equals(Coupons.coupon_name)).FirstOrDefault();
            if (coupon == null)
            {
                Session["couponerror"] = "Invalid Coupon";
            }
            else if (coupon.events.expiry_date < DateTime.Now)
            {
                Session["couponerror"] = "Coupon is Expired";
            }
            else
            {
                Session["coupon"] = coupon.coupon_name;
                Session["couponid"] = coupon.coupon_id;
                Session["coupondiscount"] = coupon.discount_val * decimal.Parse(Session["totalprice"].ToString());

            }
            return RedirectToAction("Cart");
        }

        public ActionResult RemoveCoupon()
        {
            Session["coupon"] = null;
            return RedirectToAction("Cart");
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
