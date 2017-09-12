using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QualityBags.Data;
using QualityBags.Models;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Authorization;

namespace QualityBags.Controllers
{
    [Authorize(Roles = "Admin")]
    public class BagsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IHostingEnvironment _hostingEnv;

        public BagsController(ApplicationDbContext context, IHostingEnvironment hostingEnv)
        {
            _context = context;
            _hostingEnv = hostingEnv;
        }

        // GET: Bags
        public async Task<IActionResult> Index()
        {
            var bags = _context.Bags.Include(b => b.Category).Include(b => b.Supplier).AsNoTracking();
            return View(await bags.ToListAsync());
        }

        // GET: Bags/Details/5
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

        // GET: Bags/Create
        public IActionResult Create()
        {
            PopulateCategoryDropDownList();
            PopulateSupplierDropDownList();
            return View();
        }

        // POST: Bags/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CategoryID,Description,Name,Price,SupplierID")] Bag bag, IFormFile uploadFile)
        {
            var relativeName = "";
            var fileName = "";

            if (uploadFile != null && uploadFile.Length > 0)
            {
                fileName = ContentDispositionHeaderValue.Parse(uploadFile.ContentDisposition).FileName.Trim('"');
                //Path for localhost
                relativeName = "/Images/" + DateTime.Now.ToString("ddMMyyyy-HHmmssffffff") + fileName;
                using (FileStream fs = System.IO.File.Create(_hostingEnv.WebRootPath + relativeName))
                {
                    await uploadFile.CopyToAsync(fs);
                    fs.Flush();
                }
            }
            else
            {
                relativeName = "/Images/bag1.jpg";
            }
            bag.PathOfFile = relativeName;
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(bag);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                PopulateCategoryDropDownList(bag.CategoryID);
                PopulateSupplierDropDownList(bag.SupplierID);

            }
            catch (DbUpdateException /* ex */)
            {
                //Log the error (uncomment ex variable name and write a log.
                ModelState.AddModelError("", "Unable to save changes. " +
                    "Try again, and if the problem persists " +
                    "see your system administrator.");
            }

            return View(bag);
        }

        // GET: Bags/Edit/5
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
            PopulateCategoryDropDownList(bag.CategoryID);
            PopulateSupplierDropDownList(bag.SupplierID);
            return View(bag);
        }

        // POST: Bags/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BagID,PathOfFile,CategoryID,Description,Name,Price,SupplierID")] Bag bag, IFormFile uploadFile)
        {
            var relativeName = "";
            var fileName = "";

            if (uploadFile != null && uploadFile.Length > 0)
            {
                fileName = ContentDispositionHeaderValue.Parse(uploadFile.ContentDisposition).FileName.Trim('"');
                //Path for localhost
                relativeName = "/Images/" + DateTime.Now.ToString("ddMMyyyy-HHmmssffffff") + fileName;
                using (FileStream fs = System.IO.File.Create(_hostingEnv.WebRootPath + relativeName))
                {
                    await uploadFile.CopyToAsync(fs);
                    fs.Flush();
                }
            }
            else
            {
                relativeName = bag.PathOfFile;
            }
            bag.PathOfFile = relativeName;

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
            PopulateCategoryDropDownList(bag.CategoryID);
            PopulateSupplierDropDownList(bag.SupplierID);
            return View(bag);
        }



        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int? id, IFormFile uploadFile)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var relativeName = "";
        //    var fileName = "";
        //    var editbag = await _context.Bags.SingleOrDefaultAsync(b => b.BagID == id);

        //    if (uploadFile != null && uploadFile.Length > 0)
        //    {
        //        fileName = ContentDispositionHeaderValue.Parse(uploadFile.ContentDisposition).FileName.Trim('"');
        //        //Path for localhost
        //        relativeName = "/Images/" + DateTime.Now.ToString("ddMMyyyy-HHmmssffffff") + fileName;
        //        using (FileStream fs = System.IO.File.Create(_hostingEnv.WebRootPath + relativeName))
        //        {
        //            await uploadFile.CopyToAsync(fs);
        //            fs.Flush();
        //        }
        //    }
        //    else
        //    {
        //        relativeName = "/Images/bag1.jpg";
        //    }


        //    editbag.PathOfFile = relativeName;

        //    if (await TryUpdateModelAsync<Bag>(editbag, "", 
        //        b => b.Name, b => b.Description, b => b.CategoryID, b => b.SupplierID, b => b.Price))
        //    {
        //        try
        //        {
        //            await _context.SaveChangesAsync();
        //            return RedirectToAction("Index");
        //        }
        //        catch (DbUpdateException /* ex */)
        //       {
        //            //Log the error (uncomment ex variable name and write a log.)
        //            ModelState.AddModelError("", "Unable to save changes. " +
        //                "Try again, and if the problem persists, " +
        //                "see your system administrator.");
        //        }
        //    }

        //    PopulateCategoryDropDownList(editbag.CategoryID);
        //    PopulateSupplierDropDownList(editbag.SupplierID);
        //    return View(editbag);
        //}


        //public async Task<IActionResult> Edit(int? id, IFormFile file)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var relativeName = "";
        //    var fileName = "";
        //    var editbag = await _context.Bags.SingleOrDefaultAsync(b => b.BagID == id);

        //    if (file != null && file.Length > 0)
        //    {
        //        fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
        //        //Path for localhost
        //        relativeName = "/Images/" + DateTime.Now.ToString("ddMMyyyy-HHmmssffffff") + fileName;
        //        using (FileStream fs = System.IO.File.Create(_hostingEnv.WebRootPath + relativeName))
        //        {
        //            await file.CopyToAsync(fs);
        //            fs.Flush();
        //        }
        //    }
        //    else
        //    {
        //        relativeName = "/Images/bag1.jpg";
        //    }


        //    editbag.PathOfFile = relativeName;

        //    if (await TryUpdateModelAsync<Bag>(editbag, "",
        //        b => b.Name, b => b.Description, b => b.CategoryID, b => b.SupplierID, b => b.Price, b => b.PathOfFile))
        //    {
        //        try
        //        {
        //            await _context.SaveChangesAsync();
        //            return RedirectToAction("Index");
        //        }
        //        catch (DbUpdateException /* ex */)
        //        {
        //            //Log the error (uncomment ex variable name and write a log.)
        //            ModelState.AddModelError("", "Unable to save changes. " +
        //                "Try again, and if the problem persists, " +
        //                "see your system administrator.");
        //        }
        //    }

        //    PopulateCategoryDropDownList(editbag.CategoryID);
        //    PopulateSupplierDropDownList(editbag.SupplierID);
        //    return View(editbag);
        //}

        private void PopulateCategoryDropDownList(object selectedCategory = null)
        {
            var categoriesQuery = from c in _context.Categories
                                  orderby c.Name
                                  select c;
            ViewBag.CategoryID = new SelectList(categoriesQuery.AsNoTracking(), "CategoryID", "Name", selectedCategory);
        }

        private void PopulateSupplierDropDownList(object selectedSupplier = null)
        {
            var suppliersQuery = from s in _context.Suppliers
                                 orderby s.Name
                                 select s;
            ViewBag.SupplierID = new SelectList(suppliersQuery.AsNoTracking(), "SupplierID", "Name", selectedSupplier);
        }

        // GET: Bags/Delete/5
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

        // POST: Bags/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var bag = await _context.Bags.SingleOrDefaultAsync(m => m.BagID == id);
            _context.Bags.Remove(bag);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                TempData["BagUsed"] = "The bag being deleted has been used in previous orders.Delete those orders before trying again.";
                return RedirectToAction("Delete");
            }
            return RedirectToAction("Index");
        }

        private bool BagExists(int id)
        {
            return _context.Bags.Any(e => e.BagID == id);
        }
    }
}
