using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace pointage.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            if (Session["Email"] == null)
            {
              return RedirectToAction("AdminLogin","Login");
            }
            return View();
            
        }
    }
}