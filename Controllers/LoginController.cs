using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using pointage.Model;

namespace pointage.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        EcommerceDBEntities db = new EcommerceDBEntities();
        EcommerceDBEntitiesAdmin dbadmin = new EcommerceDBEntitiesAdmin();
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(users Users)
        {
            
            var checkLogin = db.users.Where(x=>x.username.Equals(Users.username)&& x.password.Equals(Users.password)).FirstOrDefault();
            
            if (checkLogin != null)
            {
                cart Carts = db.cart.Where(x => x.users_id == checkLogin.users_id).First();
                var cartItems = db.cart_items.Where(x => x.cart_id == Carts.cart_id).Count();
                Session["cartcounter"] = cartItems.ToString();
                Session["users_id"] = Users.users_id.ToString();
                Session["username"] = Users.username.ToString();
                Session["cart_id"] = Carts.cart_id.ToString();
                return RedirectToAction("Store", "Home");

            }
            else
            {
                ViewBag.Notification = "Wrong Username or Password";
            }
            return View();

        }
        public ActionResult AdminLogin()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AdminLogin(admin Admins)
        {

            var checkAdmin = dbadmin.admins.Where(x => x.email.Equals(Admins.email) && x.password.Equals(Admins.password)).FirstOrDefault(); 
            if (checkAdmin != null)
            {
                Session["email"] = Admins.email.ToString();
                return RedirectToAction("Index", "Admin");
            }
            else
            {
                ViewBag.Notification = "Wrong Username or Password";
            }
            return View();

        }
        public ActionResult AdminLogout()
        {
            Session.Clear();
            return RedirectToAction("Store", "Home");

        }
        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Store", "Home");

        }
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(users Users)
        {
            cart Carts = new cart();
            if(db.users.Any(x=>x.username == Users.username))
            {
                ViewBag.Notification = "This account has already existed";
                return View();
            }
            else
            {
                if (ModelState.IsValid)
                {

                    Users.date_of_reg = Convert.ToDateTime(DateTime.Now.ToShortDateString());
                    Carts.users_id = Users.users_id;
                    db.cart.Add(Carts);
                    db.users.Add(Users);
                    db.SaveChanges();
                    Session["users_id"] = Users.users_id.ToString();
                    Session["username"] = Users.username.ToString();
                    Session["email"] = Users.email.ToString();
                    Session["password"] = Users.password.ToString();
                    Session["date_of_reg"] = Users.date_of_reg.ToString();
                    Session["address"] = Users.address.ToString();
                    Session["contact"] = Users.email.ToString();
                    return RedirectToAction("Login", "Login");
                }
                return View(Users);
                
            }
            
        }

    }

}