using Starcounter;

namespace ShoppingCart
{
    partial class AddToCartPage: Json, IBound<Item>
    {
        void Handle(Input.ClickTrigger action)
        {
            Db.Transact(() =>
            {
                var shopping_cart = HelperFunctions.VerifyShoppingCart(HelperFunctions.SessionID());
                HelperFunctions.AddNewShoppingCartItem(shopping_cart, this.Data, this.Quantity);
            });
        }
    }
}
