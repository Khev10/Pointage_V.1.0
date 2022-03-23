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
    public class CategoriesController : Controller
    {
        private EcommerceDBEntities db = new EcommerceDBEntities();

        // GET: Categories
        public ActionResult ViewCategory()
        {
            return View(db.category.ToList());
        }

        // GET: Categories/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            category category = db.category.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // GET: Categories/Create
        public ActionResult AddCategory()
        {
            return View();
        }

        // POST: Categories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddCategory([Bind(Include = "category_id,category_name")] category category)
        {
            if (ModelState.IsValid)
            {
                var query = from c in db.category
                            where c.category_id == category.category_id
                            select c;
                if (query.Any())
                {
                    //return Content("<script>alert('Coupin Name Already Exist'); history.go(-1)</script>");
                    ViewBag.CategoryExist = "Category ID Already Exist";
                }
                else
                {
                    db.category.Add(category);
                    db.SaveChanges();
                    return RedirectToAction("ViewCategory");
                }

            }

            return View(category);
        }

        // GET: Categories/Edit/5
        public ActionResult EditCategory(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            category category = db.category.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditCategory([Bind(Include = "category_id,category_name")] category category)
        {
            if (ModelState.IsValid)
            {
                db.Entry(category).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("ViewCategory");
            }
            return View(category);
        }

        // GET: Categories/Delete/5
        public ActionResult DeleteCategory(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            category category = db.category.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("DeleteCategory")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            category category = db.category.Find(id);
            db.category.Remove(category);
            db.SaveChanges();
            return RedirectToAction("ViewCategory");
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
