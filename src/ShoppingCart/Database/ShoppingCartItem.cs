using Starcounter;
using System;

namespace ShoppingCart
{
    [Database]
    public class ShoppingCartItem
    {
        public ShoppingCart Cart { get; set; }
        public Item CartItem { get; set; }
        public decimal ItemQuantity { get; set; }
    }
}