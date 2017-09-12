using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using QualityBags.Data;
using Microsoft.AspNetCore.Http;

namespace QualityBags.Models
{
    public class ShoppingCart
    {
        public string ShoppingCartID { get; set; }
        public const string CartSessionKey = "shoppingCartID";

        public static ShoppingCart GetCart(HttpContext context)
        {
            var cart = new ShoppingCart();
            cart.ShoppingCartID = cart.GetCartID(context);
            return cart;
        }

        public void AddToCart(Bag bag, ApplicationDbContext apContext)
        {
            var cartItem = apContext.CartItems
                .SingleOrDefault(ci => ci.CartID == ShoppingCartID && ci.Bag.BagID == bag.BagID);
            if (cartItem == null)
            {
                cartItem = new CartItem
                {
                    Bag = bag,
                    CartID = ShoppingCartID,
                    ItemCount = 1,
                };
                apContext.CartItems.Add(cartItem);
            }
            else
            {
                cartItem.ItemCount++;
            }
            apContext.SaveChanges();
        }

        public int RemoveFromCart(int bagID, ApplicationDbContext apContext)
        {
            var cartItem = apContext.CartItems.SingleOrDefault(ci => ci.CartID == ShoppingCartID && ci.Bag.BagID == bagID);
            int itemCount = 0;

            if (cartItem != null)
            {
                if (cartItem.ItemCount > 1)
                {
                    cartItem.ItemCount--;
                    itemCount = cartItem.ItemCount;
                }
                else
                {
                    apContext.CartItems.Remove(cartItem);
                }
                apContext.SaveChanges();
            }
            return itemCount;
        }

        public void ClearCart(ApplicationDbContext apContext)
        {
            var cartItems = apContext.CartItems.Where(ci => ci.CartID == ShoppingCartID);
            foreach (var cartItem in cartItems)
            {
                apContext.CartItems.Remove(cartItem);
            }
            apContext.SaveChanges();
        }

        public List<CartItem> GetCartItems(ApplicationDbContext apContext)
        {
            List<CartItem> cartItems = apContext.CartItems.Include(ci => ci.Bag).ThenInclude(b => b.Category).Where(ci => ci.CartID == ShoppingCartID).ToList();
            return cartItems;
        }

        public int GetTotalCount(ApplicationDbContext apContext)
        {
            int? totalCount =
                (from cartItem in apContext.CartItems where cartItem.CartID == ShoppingCartID select (int?)cartItem.ItemCount).Sum();
            return totalCount ?? 0;
        }

        public decimal GetSubtotal(ApplicationDbContext apContext)
        {
            decimal? subtotal = (from cartItem in apContext.CartItems
                              where cartItem.CartID == ShoppingCartID
                              select (int?)cartItem.ItemCount * cartItem.Bag.Price).Sum();
            return subtotal ?? decimal.Zero;
        }

        public decimal GetTotalGST(ApplicationDbContext apContext)
        {
            decimal? totalGST = GetSubtotal(apContext) * Convert.ToDecimal(0.15);
            return totalGST ?? decimal.Zero;
        }

        public decimal GetGrandTotal(ApplicationDbContext apContext)
        {
            decimal? totalAmount = GetSubtotal(apContext) * Convert.ToDecimal(1.15);
            return totalAmount ?? decimal.Zero;
        }

        public string GetCartID(HttpContext context)
        {
            if (context.Session.GetString(CartSessionKey) == null)
            {
                Guid tempCartId = Guid.NewGuid();
                context.Session.SetString(CartSessionKey, tempCartId.ToString());
            }
            return context.Session.GetString(CartSessionKey).ToString();
        }

    }
}
