using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace pointage.Model
{
    public class ShoppingCart
    {
        public product product { get; set; }
        public int Quantity { get; set; }
        public ShoppingCart(product products, int quantity)
        {
            product = products;
            Quantity = quantity;

        }


    }
}