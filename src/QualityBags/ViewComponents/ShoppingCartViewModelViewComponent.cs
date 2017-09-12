using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QualityBags.Data;
using QualityBags.Models;
using QualityBags.Models.ShoppingCartViewModels;
using Microsoft.AspNetCore.Mvc;

namespace QualityBags.ViewComponents
{
    public class ShoppingCartViewModelViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;

        public ShoppingCartViewModelViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke()
        {
            return View(ReturnCurrentCartViewModel());
        }

        public ShoppingCartViewModel ReturnCurrentCartViewModel()
        {
            var cart = ShoppingCart.GetCart(this.HttpContext);
            // Set up our ViewModel
            var viewModel = new ShoppingCartViewModel
            {
                CartItems = cart.GetCartItems(_context),
                SubTotal = cart.GetSubtotal(_context),
                GST = cart.GetTotalGST(_context),
                GrandTotal = cart.GetGrandTotal(_context),
                TotalCount = cart.GetTotalCount(_context)
            };
            
            return viewModel;
        }


    }
}
