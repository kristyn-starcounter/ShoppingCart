using Starcounter;
using System.Collections.Generic;
using System.Linq;
using System;

namespace ShoppingCart
{
    partial class ShoppingCartPage: Json, IBound<ShoppingCart>
    {
        static ShoppingCartPage()
        {
            DefaultTemplate.SelectedItemName.Bind = nameof(SelectedItemNameString);
        }

        public decimal TotalPrice => GetAllShoppingCartItems(SessionID()).Select(O => O.ItemQuantity * O.CartItem.ItemPrice).Sum();
        public decimal TotalItems => GetAllShoppingCartItems(SessionID()).Select(O => O.ItemQuantity).Sum();
        public IEnumerable<Item> Items => GetAllItems();
        public IEnumerable<ShoppingCartItem> ShoppingCartItems => GetAllShoppingCartItems(SessionID());
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
                this.SelectedItem = Items.FirstOrDefault().ItemName;
            }

            Db.Transact(() =>
            {
                if (GetShoppingCart(SessionID()) == null)
                {
                    new ShoppingCart { ShoppingCartSessionID = SessionID() };
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

            var shopping_cart_item = GetShoppingCartItem(SessionID(), SelectedItemNameString);
            var item = GetItem(SelectedItemNameString);

            if (shopping_cart_item == null)
            {
                Db.Transact(() =>
                {
                    ShoppingCart shopping_cart = VerifyShoppingCart(SessionID());
                    new ShoppingCartItem { Cart = shopping_cart, CartItem = item, ItemQuantity = 1 };
                });
            }
        }

        public void RemoveItem(ShoppingItemsCart shopping_cart_item)
        {
            Db.Transact(() =>
            {
                RemoveShoppingCartItem(SessionID(), shopping_cart_item.CartItem.ItemName);
            });
        }

        public void Handle(Input.ClearAllItemsTrigger action)
        {
            Db.Transact(() =>
            {
                RemoveShoppingCartItems(SessionID());
            });
        }

        //-----------------------------------------------------------------------------------------------------------------------------
        //Tools
        public string SessionID()
        {
            //return "1";
            return Session.Current.SessionId;
        }

        public IEnumerable<Item> GetAllItems()
        {
            return Db.SQL<Item>($"SELECT i FROM Item i");
        }

        public Item GetItem(string name)
        {
            return Db.SQL<Item>("SELECT i FROM Item i WHERE i.ItemName = ?", name).FirstOrDefault();
        }

        public ShoppingCart VerifyShoppingCart(string session_id)
        {
            ShoppingCart shopping_cart = Db.SQL<ShoppingCart>("SELECT s FROM ShoppingCart s WHERE s.ShoppingCartSessionID = ?", session_id).FirstOrDefault();

            if (shopping_cart == null)
            {
                shopping_cart = new ShoppingCart { ShoppingCartSessionID = session_id };
            }

            return shopping_cart;
        }

        public ShoppingCart GetShoppingCart(string session_id)
        {
            return Db.SQL<ShoppingCart>("SELECT s FROM ShoppingCart s WHERE s.ShoppingCartSessionID = ?", session_id).FirstOrDefault();
        }

        public IEnumerable<ShoppingCartItem> GetAllShoppingCartItems(string session_id)
        {
            return Db.SQL<ShoppingCartItem>("SELECT s FROM ShoppingCartItem s WHERE s.Cart.ShoppingCartSessionID = ?", session_id);
        }

        public ShoppingCartItem GetShoppingCartItem(string session_id, string item_name)
        {
            return Db.SQL<ShoppingCartItem>("SELECT s FROM ShoppingCartItem s WHERE s.Cart.ShoppingCartSessionID = ? AND s.CartItem.ItemName = ?", session_id, item_name).FirstOrDefault();
        }

        public bool DoesShoppingCartItemExist(ShoppingCart cart, Item item)
        {
            bool does_exist = false;
            if (Db.SQL<ShoppingCartItem>($"SELECT s FROM ShoppingCartItem s WHERE s.Cart = ?  AND s.Item = ?", cart, item).FirstOrDefault() != null)
            {
                does_exist = true;
            }

            return does_exist;
        }

        public bool AddNewShoppingCartItem(ShoppingCart cart, Item item, Int64 ItemQuantity)
        {
            bool is_added = false;

            if (!DoesShoppingCartItemExist(cart, item))
            {
                new ShoppingCartItem { Cart = cart, CartItem = item, ItemQuantity = 1 };
            }

            return is_added;
        }

        public void RemoveShoppingCartItems(string session_id)
        {
            Db.SQL("DELETE FROM ShoppingCartItem WHERE Cart.ShoppingCartSessionID = ?", session_id);
        }

        public void RemoveShoppingCartItem(string session_id, string item_name)
        {
            Db.SQL("DELETE FROM ShoppingCartItem WHERE Cart.ShoppingCartSessionID = ? AND CartItem.ItemName = ?", session_id, item_name);
        }
    }

    [ShoppingCartPage_json.ShoppingCartItems]
    partial class ShoppingItemsCart: Json, IBound<ShoppingCartItem>
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
