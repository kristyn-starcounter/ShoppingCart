using Starcounter;
using System.Collections.Generic;

namespace ShoppingCart
{
    [Database]
    public class Item
    {
        public string Key => this.GetObjectID();
        public string ItemName { get; set; }
        public decimal ItemPrice { get; set; }

        public IEnumerable<ShoppingCartItem> ShoppingCarts =>
            Db.SQL<ShoppingCartItem>("SELECT s.Cart FROM ShoppingCartItem WHERE s.CartItem = ?", this);
    }
}
