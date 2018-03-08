using Starcounter;
using System.Collections.Generic;

namespace ShoppingCart
{
    [Database]
    public class ShoppingCart
    {
        public string Key => this.GetObjectID();
        public string ShoppingCartSessionID { get; set; }

        public IEnumerable<ShoppingCartItem> ShoppingCarts =>
            Db.SQL<ShoppingCartItem>("SELECT s.CartItem FROM ShoppingCartItem WHERE s.Cart = ?", this);
    }
}