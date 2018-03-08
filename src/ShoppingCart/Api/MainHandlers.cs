using Starcounter;

namespace ShoppingCart
{
    static class MainHandlers
    {
        public static void Register()
        {
            Handle.GET("/ShoppingCart", () =>
            {
                return new ShoppingCartPage { Data = null };
            });
        }
    }
}
