using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QualityBags.Data;
using QualityBags.Models;
using Microsoft.AspNetCore.Authorization;

namespace QualityBags.Controllers
{
    [AllowAnonymous]
    [Authorize(Roles = "Admin, Member")]
    public class OrderBagsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrderBagsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: OrderBags
        public async Task<IActionResult> Index(string SearchString, string currentFilter, int? id, int? page)
        {
            ViewBag.Categories = _context.Categories.ToList();

            if (SearchString != null)
            {
                page = 1;
            }
            else
            {
                SearchString = currentFilter;
            }

            ViewData["CurrentFilter"] = SearchString;
            var bags = from b in _context.Bags.Include(b => b.Category)
                       select b;

            if (id != null)
            {
                bags = bags.Where(b => b.CategoryID == id);
            }

            if (!String.IsNullOrEmpty(SearchString))
            {
                bags = bags.Where(b => b.Name.Contains(SearchString));
            }

            int pageSize = 8;
            return View(await PaginatedList<Bag>.CreateAsync(bags.AsNoTracking(), page ?? 1, pageSize));
        }

        // GET: OrderBags/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bag = await _context.Bags.SingleOrDefaultAsync(m => m.BagID == id);
            if (bag == null)
            {
                return NotFound();
            }

            return View(bag);
        }


        // GET: OrderBags/Create
        public IActionResult Create()
        {
            ViewData["CategoryID"] = new SelectList(_context.Categories, "CategoryID", "Name");
            ViewData["SupplierID"] = new SelectList(_context.Suppliers, "SupplierID", "EmailAddress");
            return View();
        }

        // POST: OrderBags/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BagID,CategoryID,Description,Name,PathOfFile,Price,SupplierID")] Bag bag)
        {
            if (ModelState.IsValid)
            {
                _context.Add(bag);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewData["CategoryID"] = new SelectList(_context.Categories, "CategoryID", "Name", bag.CategoryID);
            ViewData["SupplierID"] = new SelectList(_context.Suppliers, "SupplierID", "EmailAddress", bag.SupplierID);
            return View(bag);
        }

        // GET: OrderBags/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bag = await _context.Bags.SingleOrDefaultAsync(m => m.BagID == id);
            if (bag == null)
            {
                return NotFound();
            }
            ViewData["CategoryID"] = new SelectList(_context.Categories, "CategoryID", "Name", bag.CategoryID);
            ViewData["SupplierID"] = new SelectList(_context.Suppliers, "SupplierID", "EmailAddress", bag.SupplierID);
            return View(bag);
        }

        // POST: OrderBags/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BagID,CategoryID,Description,Name,PathOfFile,Price,SupplierID")] Bag bag)
        {
            if (id != bag.BagID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(bag);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BagExists(bag.BagID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            ViewData["CategoryID"] = new SelectList(_context.Categories, "CategoryID", "Name", bag.CategoryID);
            ViewData["SupplierID"] = new SelectList(_context.Suppliers, "SupplierID", "EmailAddress", bag.SupplierID);
            return View(bag);
        }

        // GET: OrderBags/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bag = await _context.Bags.SingleOrDefaultAsync(m => m.BagID == id);
            if (bag == null)
            {
                return NotFound();
            }

            return View(bag);
        }

        // POST: OrderBags/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var bag = await _context.Bags.SingleOrDefaultAsync(m => m.BagID == id);
            _context.Bags.Remove(bag);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool BagExists(int id)
        {
            return _context.Bags.Any(e => e.BagID == id);
        }

    }
}
