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

            Handle.GET("/ShoppingCart/Partials/Item/{?}", (string objectId) =>
            {
                return AddItemToCart(objectId);
            });

            Handle.GET("/ShoppingCart/Partials/AddItem1/{?}", (string objectId) =>
            {
                return AddItemToCart(objectId);
            });

            Handle.GET("/ShoppingCart/Partials/AddItem2/{?}", (string objectId) =>
            {
                return AddItemToCart(objectId);
            });

            AddToCartPage AddItemToCart(string objectId)
            {
                return new AddToCartPage() { Data = Db.FromId<Item>(objectId) };
            }            
        }
    }
}
