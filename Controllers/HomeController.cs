using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Web;
using System.Web.Mvc;
using pointage.Model;

namespace pointage.Controllers
{

    public class HomeController : Controller
    {
        private EcommerceDBEntities db = new EcommerceDBEntities();
     
        public ActionResult Store()
        {
            var products = db.product.Include(p => p.category);
            products = products.Where(p => p.product_available_qty != 0);
            return View(products.ToList());
        }
        public ActionResult Checkout()
        {
            return View();
        }

        public ActionResult Complete()
        {
            return View();
        }
        public ActionResult Contacts()
        {
            return View();
        }
        public ActionResult Cart()
        {
            if (Session["cart_id"] == null) {
                return RedirectToAction("Login", "Login");
            }
            return View();
        }
        public ActionResult AddToCart()
        {

            return RedirectToAction("Store", "Home");
        }
    }
}