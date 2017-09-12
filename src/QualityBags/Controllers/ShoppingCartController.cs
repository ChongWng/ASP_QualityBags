using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using QualityBags.Data;
using QualityBags.Models;
using QualityBags.Models.ShoppingCartViewModels;
using Microsoft.AspNetCore.Authorization;

namespace QualityBags.Controllers
{
    [AllowAnonymous]
    [Authorize(Roles = "Member")]
    public class ShoppingCartController : Controller
    {
        ApplicationDbContext _context;

        public ShoppingCartController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var cart = ShoppingCart.GetCart(this.HttpContext);
            // Return the view
            return View(cart);
        }

        // GET: /Store/AddToCart/5
        public ActionResult AddToCart(int id)
        {
            // Retrieve the bag from the database
            var addedBagItem = _context.Bags
                .Single(b => b.BagID == id);
            // Add it to the shopping cart
            var cart = ShoppingCart.GetCart(this.HttpContext);
            cart.AddToCart(addedBagItem, _context);
            // Go back to the main store page for more shopping
            //return RedirectToAction("Index", "OrderBags");
            return Redirect(Request.Headers["Referer"].ToString());
        }

        public ActionResult RemoveFromCart(int id)
        {
            var cart = ShoppingCart.GetCart(this.HttpContext);
            int itemCount = cart.RemoveFromCart(id, _context);
            return Redirect(Request.Headers["Referer"].ToString()); 
        }

        public ActionResult ClearCart()
        {
            var cart = ShoppingCart.GetCart(this.HttpContext);
            cart.ClearCart(_context);
            return Redirect(Request.Headers["Referer"].ToString());
        }
    }
}