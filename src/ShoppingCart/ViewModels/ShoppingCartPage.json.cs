using Starcounter;
using System.Collections.Generic;
using System.Linq;
using System;

namespace ShoppingCart
{
    partial class ShoppingCartPage: Json, IBound<Cart>
    {
        static ShoppingCartPage()
        {
            DefaultTemplate.SelectedItemName.Bind = nameof(SelectedItemNameString);
        }

        public decimal TotalPrice => HelperFunctions.GetAllCartItems(HelperFunctions.SessionID()).Select(O => O.ItemQuantity * O.Item.ItemPrice).Sum();
        public decimal TotalItems => HelperFunctions.GetAllCartItems(HelperFunctions.SessionID()).Select(O => O.ItemQuantity).Sum();
        public IEnumerable<Item> Items => HelperFunctions.GetAllItems();
        public IEnumerable<CartItem> CartItems => HelperFunctions.GetAllCartItems(HelperFunctions.SessionID());
        public string SelectedItemNameString => $"{ SelectedItem}";

        protected override void OnData()
        {
            base.OnData();

            if (Session.Current == null)
            {
                Session.Ensure();
            }

            if (this.SelectedItem == "")
            {
                if (Items.FirstOrDefault() != null)
                {
                    this.SelectedItem = Items.FirstOrDefault().ItemName;
                }
            }

            Db.Transact(() =>
            {
                if (HelperFunctions.GetShoppingCart(HelperFunctions.SessionID()) == null)
                {
                    new Cart { ShoppingCartSessionID = HelperFunctions.SessionID() };
                }
            });

            return;
        }

        public void Handle(Input.AddItemTrigger action)
        {
            if (string.IsNullOrEmpty(SelectedItemNameString))
            {
                return;
            }

            var shopping_cart_item = HelperFunctions.GetShoppingCartItem(HelperFunctions.SessionID(), SelectedItemNameString);
            var item = HelperFunctions.GetItem(SelectedItemNameString);

            if (shopping_cart_item == null)
            {
                Db.Transact(() =>
                {
                    Cart shopping_cart = HelperFunctions.VerifyShoppingCart(HelperFunctions.SessionID());
                    HelperFunctions.AddNewShoppingCartItem(shopping_cart, item, 1.0M);

                });
            }
        }

        public void RemoveItem(ShoppingItemsCart shopping_cart_item)
        {
            Db.Transact(() =>
            {
                HelperFunctions.RemoveShoppingCartItem(HelperFunctions.SessionID(), shopping_cart_item.Item.ItemName);
            });
        }

        public void Handle(Input.ClearAllItemsTrigger action)
        {
            Db.Transact(() =>
            {
                HelperFunctions.RemoveCartItems(HelperFunctions.SessionID());
            });
        }        
    }

    [ShoppingCartPage_json.CartItems]
    partial class ShoppingItemsCart: Json, IBound<CartItem>
    {
        void Handle(Input.RemoveItemTrigger action)
        {
            var ShoppingCartPage = (ShoppingCartPage)Parent.Parent;
            ShoppingCartPage.RemoveItem(this);
        }

        void Handle(Input.ItemQuantity action)
        {
            Db.Transact(() =>
            {
                var ShoppingCartPage = (ShoppingCartPage)Parent.Parent;
                if (action.Value > 0)
                {
                    ItemQuantity = action.Value;
                }
                else
                {
                    ShoppingCartPage.RemoveItem(this);
                }
            });
            action.Cancel();
        }

    }

}
