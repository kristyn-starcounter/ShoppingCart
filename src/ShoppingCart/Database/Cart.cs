using Starcounter;
using System.Collections.Generic;

namespace ShoppingCart
{
    [Database]
    public class Cart
    {
        public string Key => this.GetObjectID();

        public string ShoppingCartSessionID { get; set; }
        
        public Customer Customer { get; set; }

        public IEnumerable<CartItem> ShoppingCarts =>
            Db.SQL<CartItem>("SELECT s.CartItem FROM ShoppingCartItem WHERE s.Cart = ?", this);
    }
}