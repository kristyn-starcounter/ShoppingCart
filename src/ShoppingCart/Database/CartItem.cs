using Starcounter;
using System;

namespace ShoppingCart
{
    [Database]
    public class CartItem
    {
        public Cart Cart { get; set; }
        public Item Item { get; set; }
        public decimal ItemQuantity { get; set; }
    }
}