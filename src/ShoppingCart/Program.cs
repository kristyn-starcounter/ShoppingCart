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
            Application.Current.Use(new HtmlFromJsonProvider());
            Application.Current.Use(new PartialToStandaloneHtmlProvider());
            MainHandlers.Register();
            BlenderMapping.Register();
        }
    }
}