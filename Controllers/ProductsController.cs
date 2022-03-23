using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.IO;
using pointage.Model;

namespace pointage.Controllers
{
    public class ProductsController : Controller
    {
        private EcommerceDBEntities db = new EcommerceDBEntities();

        // GET: Products
        public ActionResult ViewProducts()
        {
            var products = db.product.Include(p => p.category);
            return View(products.ToList());
        }

        // GET: Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            product product = db.product.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: Products/Create
        public ActionResult AddProduct()
        {
            ViewBag.category_id = new SelectList(db.category, "category_id", "category_name");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddProduct([Bind(Include = "product_id,product_name,category_id,product_price,product_available_qty,ImageFile")] product product)
        {
            if (product.ImageFile != null)
            {
                string filename = Path.GetFileNameWithoutExtension(product.ImageFile.FileName);
                string extension = Path.GetExtension(product.ImageFile.FileName);
                filename += DateTime.Now.ToString("yymmddss") + extension;
                product.product_image = filename;
                filename = Path.Combine(Server.MapPath("~/Img/product/"), filename);
                product.ImageFile.SaveAs(filename);
                if (ModelState.IsValid)
                {
                    db.product.Add(product);
                    db.SaveChanges();
                    return RedirectToAction("ViewProducts");
                }
            }
            else
            {
                ViewBag.ImageNotExist = "Please Select Image";
            }

            ViewBag.category_id = new SelectList(db.category, "category_id", "category_name", product.category_id);
            return View(product);
        }

        // GET: Products/Edit/5
        public ActionResult EditProduct(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            product product = db.product.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.category_id = new SelectList(db.category, "category_id", "category_name", product.category_id);
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditProduct([Bind(Include = "product_id,product_name,category_id,product_price,product_available_qty,product_image,ImageFile")] product product)
        {
            if (product.ImageFile != null)
            {
                System.IO.File.Delete(Path.Combine(Server.MapPath("~/Img/product/"),product.product_image));
                string filename = Path.GetFileNameWithoutExtension(product.ImageFile.FileName);
                string extension = Path.GetExtension(product.ImageFile.FileName);
                filename += DateTime.Now.ToString("yymmddss") + extension;
                product.product_image = filename;
                filename = Path.Combine(Server.MapPath("~/Img/product/"), filename);
                product.ImageFile.SaveAs(filename);
                if (ModelState.IsValid)
                {
                    db.Entry(product).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("ViewProducts");
                }
            }
            else
            {
                if (ModelState.IsValid)
                {
                    db.Entry(product).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("ViewProducts");
                }
            }
            ViewBag.category_id = new SelectList(db.category, "category_id", "category_name", product.category_id);
            return View(product);
        }

        // GET: Products/Delete/5
        public ActionResult DeleteProduct(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            product product = db.product.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("DeleteProduct")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            product product = db.product.Find(id);
            System.IO.File.Delete(Path.Combine(Server.MapPath("~/Img/product/"), product.product_image));
            db.product.Remove(product);
            db.SaveChanges();
            return RedirectToAction("ViewProducts");
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
