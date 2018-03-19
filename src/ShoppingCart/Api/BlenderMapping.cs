using Starcounter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart
{
    static class BlenderMapping
    {
        public static void Register()
        {
            Blender.MapUri2<Item>("/ShoppingCart/Partials/Item/{?}", new[] {"small"});
            Blender.MapUri2<Item>("/ShoppingCart/Partials/AddItem1/{?}", new[] { "page" });
            Blender.MapUri2<Item>("/ShoppingCart/Partials/AddItem2/{?}", new[] { "detail" });
        }
    } 
}
