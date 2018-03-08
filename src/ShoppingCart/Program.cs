using System;
using Starcounter;
using System.Collections.Generic;
using System.Linq;

namespace ShoppingCart
{
    class Program
    {
        static void Main()
        {
            Db.Transact(() =>
            {
                Db.SQL("DELETE FROM Item");
                Db.SQL("DELETE FROM ShoppingCart");
                Db.SQL("DELETE FROM ShoppingCartItem");
            });

            Db.Transact(() =>
            {
                new Item { ItemName = "Kangaroo", ItemPrice = 314.00M };
                new Item { ItemName = "Penguin", ItemPrice = 73.00M };
                new Item { ItemName = "Lion", ItemPrice = 109.00M };
                new Item { ItemName = "Gator", ItemPrice = 803.00M };
                new Item { ItemName = "Ostrich", ItemPrice = 573.00M };
                new Item { ItemName = "Racoon", ItemPrice = 200.00M };
            });

            Application.Current.Use(new HtmlFromJsonProvider());
            Application.Current.Use(new PartialToStandaloneHtmlProvider());


            MainHandlers.Register();
        }
    }
}