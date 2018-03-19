using Starcounter;
using System.Collections.Generic;
using System.Linq;
using System;

namespace ShoppingCart
{
    static class HelperFunctions
    {
       static public string SessionID()
        {
            return "1";
            //return Session.Current.SessionId;
        }

        static public IEnumerable<Item> GetAllItems()
        {
            return Db.SQL<Item>($"SELECT i FROM {nameof(Item)} i");
        }

        static public Item GetItem(string name)
        {
            return Db.SQL<Item>($"SELECT i FROM {nameof(Item)} i WHERE i.{nameof(Item.ItemName)} = ?", name).FirstOrDefault();
        }

        static public Cart VerifyShoppingCart(string session_id)
        {
            Cart shopping_cart = Db.SQL<Cart>($"SELECT s FROM {nameof(Cart)} s WHERE s.{nameof(Cart.ShoppingCartSessionID)} = ?", session_id).FirstOrDefault();

            if (shopping_cart == null)
            {
                shopping_cart = new Cart { ShoppingCartSessionID = session_id };
            }

            return shopping_cart;
        }

        static public Cart GetShoppingCart(string session_id)
        {
            return Db.SQL<Cart>($"SELECT s FROM {nameof(Cart)} s WHERE s.{nameof(Cart.ShoppingCartSessionID)} = ?", session_id).FirstOrDefault();
        }

        static public IEnumerable<CartItem> GetAllCartItems(string session_id)
        {
            return Db.SQL<CartItem>($"SELECT s FROM {nameof(CartItem)} s WHERE s.{nameof(CartItem.Cart)}.{nameof(Cart.ShoppingCartSessionID)} = ?", session_id);
        }

        static public CartItem GetShoppingCartItem(string session_id, string item_name)
        {
            return Db.SQL<CartItem>($"SELECT s FROM {nameof(CartItem)} s WHERE s.{nameof(CartItem.Cart)}.{nameof(Cart.ShoppingCartSessionID)} = ? AND s.{nameof(CartItem.Item)}.{nameof(Item.ItemName)} = ?", session_id, item_name).FirstOrDefault();
        }

        static public bool DoesShoppingCartItemExist(Cart cart, Item item)
        {
            bool does_exist = false;
            if (Db.SQL<CartItem>($"SELECT s FROM {nameof(CartItem)} s WHERE s.{nameof(CartItem.Cart)} = ?  AND s.{nameof(CartItem.Item)} = ?", cart, item).FirstOrDefault() != null)
            {
                does_exist = true;
            }

            return does_exist;
        }

        static public bool AddNewShoppingCartItem(Cart cart, Item item, decimal item_quantity)
        {
            bool is_added = false;

            if (!DoesShoppingCartItemExist(cart, item))
            {
                new CartItem { Cart = cart, Item = item, ItemQuantity = item_quantity };
            }

            return is_added;
        }

        static public void RemoveCartItems(string session_id)
        {
            Db.SQL($"DELETE FROM {nameof(CartItem)} WHERE {nameof(CartItem.Cart)}.{nameof(Cart.ShoppingCartSessionID)} = ?", session_id);
        }

        static public void RemoveShoppingCartItem(string session_id, string item_name)
        {
            Db.SQL($"DELETE FROM {nameof(CartItem)} WHERE {nameof(CartItem.Cart)}.{nameof(Cart.ShoppingCartSessionID)} = ? AND {nameof(CartItem.Item)}.{nameof(Item.ItemName)} = ?", session_id, item_name);
        }
    }   

}
