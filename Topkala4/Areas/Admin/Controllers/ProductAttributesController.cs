using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DataLayer;

namespace TopKala3.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductAttributesController : Controller
    {
        private readonly TopKala3Contex _context;

        public ProductAttributesController(TopKala3Contex context)
        {
            _context = context;
        }

        // GET: Admin/ProductAttributes
        public async Task<IActionResult> Index()
        {
            var topKala3Contex = _context.ProductAttributes.Include(p => p.attribute).Include(p => p.product);
            return View(await topKala3Contex.ToListAsync());
        }

        // GET: Admin/ProductAttributes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productAttribute = await _context.ProductAttributes
                .Include(p => p.attribute)
                .Include(p => p.product)
                .FirstOrDefaultAsync(m => m.ProductAttributeId == id);
            if (productAttribute == null)
            {
                return NotFound();
            }

            return View(productAttribute);
        }

        // GET: Admin/ProductAttributes/Create
        public IActionResult Create()
        {
            ViewData["AttributeId"] = new SelectList(_context.Attributes, "AttributeId", "title");
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "Name");
            return View();
        }

        // POST: Admin/ProductAttributes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductAttributeId,AttributeId,ProductId,Value")] ProductAttribute productAttribute)
        {
            if (ModelState.IsValid)
            {
                _context.Add(productAttribute);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AttributeId"] = new SelectList(_context.Attributes, "AttributeId", "title", productAttribute.AttributeId);
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "Name", productAttribute.ProductId);
            return View(productAttribute);
        }

        // GET: Admin/ProductAttributes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productAttribute = await _context.ProductAttributes.FindAsync(id);
            if (productAttribute == null)
            {
                return NotFound();
            }
            ViewData["AttributeId"] = new SelectList(_context.Attributes, "AttributeId", "title", productAttribute.AttributeId);
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "Name", productAttribute.ProductId);
            return View(productAttribute);
        }

        // POST: Admin/ProductAttributes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductAttributeId,AttributeId,ProductId,Value")] ProductAttribute productAttribute)
        {
            if (id != productAttribute.ProductAttributeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(productAttribute);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductAttributeExists(productAttribute.ProductAttributeId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["AttributeId"] = new SelectList(_context.Attributes, "AttributeId", "title", productAttribute.AttributeId);
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "Name", productAttribute.ProductId);
            return View(productAttribute);
        }

        // GET: Admin/ProductAttributes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productAttribute = await _context.ProductAttributes
                .Include(p => p.attribute)
                .Include(p => p.product)
                .FirstOrDefaultAsync(m => m.ProductAttributeId == id);
            if (productAttribute == null)
            {
                return NotFound();
            }

            return View(productAttribute);
        }

        // POST: Admin/ProductAttributes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var productAttribute = await _context.ProductAttributes.FindAsync(id);
            _context.ProductAttributes.Remove(productAttribute);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductAttributeExists(int id)
        {
            return _context.ProductAttributes.Any(e => e.ProductAttributeId == id);
        }
    }
}
