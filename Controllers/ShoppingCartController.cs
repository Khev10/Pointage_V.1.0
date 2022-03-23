using pointage.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace pointage.Controllers
{
    public class ShoppingCartController : Controller
    {
        private EcommerceDBEntities db = new EcommerceDBEntities();
        private string strshoppingcart = "ShoppingCart";
        // GET: ShoppingCart
        public ActionResult Index()
        {

            return View();
        }
        public ActionResult OrderNow(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            if (Session[strshoppingcart] == null)
            {
                List<ShoppingCart> lsShoppingCart = new List<ShoppingCart>
                {
                    new ShoppingCart(db.product.Find(id),1)
                };
                Session[strshoppingcart] = lsShoppingCart;
               
            }
            else
            {
                List<ShoppingCart> lsShoppingCart = (List<ShoppingCart>)Session[strshoppingcart];
                int check=isExistingCheck(id);
                if (check == -1)
                    
                    lsShoppingCart.Add(new ShoppingCart(db.product.Find(id), 1));
                else
                    lsShoppingCart[check].Quantity++;

               
                Session[strshoppingcart] = lsShoppingCart;
            }
            return View("Index");
        }
        private int isExistingCheck(int? id)
        {
            List<ShoppingCart> lsShoppingCart = (List<ShoppingCart>)Session[strshoppingcart];
            for (int i=0; i < lsShoppingCart.Count; i++)
            {
                if (lsShoppingCart[i].product.product_id == id) return i;
            }
            return -1;
        }
        public ActionResult Delete(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            int check =isExistingCheck(id);
            List<ShoppingCart> lsShoppingCart = (List<ShoppingCart>)Session[strshoppingcart];
            lsShoppingCart.RemoveAt(check);
            return View("Index");
        }
    }
}