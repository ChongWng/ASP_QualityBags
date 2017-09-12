using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QualityBags.Data;
using QualityBags.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Authorization;

namespace QualityBags.Controllers
{
    [Authorize(Roles = "Admin, Member")]

    public class OrdersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private UserManager<ApplicationUser> _userManager;

        public OrdersController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Orders
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Orders.Include(i => i.User).AsNoTracking().ToListAsync());
        }

        // GET: Orders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.User)
                .Include(o => o.OrderItems)
                .ThenInclude(o => o.Bag)
                .ThenInclude(b => b.Category)
                .SingleOrDefaultAsync(o => o.OrderID == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // GET: Orders/Create
        [Authorize(Roles = "Admin, Member")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("Create")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Member")]
        public async Task<IActionResult> CreatePost()
        {
            var order = new Order();
            ApplicationUser user = await _userManager.GetUserAsync(User);

            if (ModelState.IsValid)
            {
                ShoppingCart cart = ShoppingCart.GetCart(this.HttpContext);
                List<CartItem> cartItems = cart.GetCartItems(_context);
                List<OrderItem> orderItems = new List<OrderItem>();
                foreach (CartItem ci in cartItems)
                {
                    OrderItem oi = CreateOrderDetailForThisItem(ci);
                    oi.Order = order;
                    orderItems.Add(oi);
                    _context.Add(oi);

                }

                order.User = user;
                order.Date = DateTime.Today;
                order.OrderStatus = OrderStatus.Waiting;
                order.SubTotal = ShoppingCart.GetCart(this.HttpContext).GetSubtotal(_context);
                order.GST = ShoppingCart.GetCart(this.HttpContext).GetTotalGST(_context);
                order.GrandTotal = ShoppingCart.GetCart(this.HttpContext).GetGrandTotal(_context);
                order.OrderItems = orderItems;
                _context.SaveChanges();

                return RedirectToAction("Purchased", new RouteValueDictionary(
                new { action = "Purchased", id = order.OrderID }));
            }

            return View(order);
        }

        private OrderItem CreateOrderDetailForThisItem(CartItem item)
        {

            OrderItem oi = new OrderItem();
            oi.Quantity = item.ItemCount;
            oi.Bag = item.Bag;
            oi.OrderItemPrice = item.Bag.Price;

            return oi;
        }

        public async Task<IActionResult> Purchased(int? id)
        {
            //await CreatePost();

            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.User)
                .AsNoTracking()
                .SingleOrDefaultAsync(o => o.OrderID == id);
            if (order == null)
            {
                return NotFound();
            }

            var orderItems = _context.OrderItems
                .Where(oi => oi.Order.OrderID == order.OrderID)
                .Include(oi => oi.Bag)
                .ThenInclude(b => b.Category)
                .ToList();

            order.OrderItems = orderItems;
            ShoppingCart.GetCart(this.HttpContext).ClearCart(_context);
            return View(order);
        }


        // GET: Orders/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.User)
                .AsNoTracking()
                .SingleOrDefaultAsync(o => o.OrderID == id);

            if (order == null)
            {
                return NotFound();
            }

            var orderItems = _context.OrderItems
                .Where(oi => oi.Order.OrderID == order.OrderID)
                .Include(oi => oi.Bag)
                .ToList();

            order.OrderItems = orderItems;

            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var order = await _context.Orders.SingleOrDefaultAsync(m => m.OrderID == id);
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        // GET: Orders
        [Authorize(Roles = "Admin, Member")]
        //public async Task<IActionResult> CustomerIndex()
        //{
        //    return View(await _context.Orders.Include(i => i.User).AsNoTracking().ToListAsync());
        //}

        public async Task<IActionResult> CustomerIndex()
        {
            ApplicationUser user = await _userManager.GetUserAsync(User);
            return View(await _context.Orders.Where(o => o.User == user).Include(o => o.User).AsNoTracking().ToListAsync());
        }

        // GET: Orders/Details/5
        [Authorize(Roles = "Admin, Member")]
        public async Task<IActionResult> CustomerDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.User)
                .Include(o => o.OrderItems)
                .ThenInclude(o => o.Bag)
                .ThenInclude(b => b.Category)
                .SingleOrDefaultAsync(o => o.OrderID == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }
    }
}
