using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DataLayer;

namespace TopKala2.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductColorsController : Controller
    {
        private readonly TopKala3Contex _context;

        public ProductColorsController(TopKala3Contex context)
        {
            _context = context;
        }
        public IActionResult ColorList(int id)
        {
            ViewBag.productid = id;
            var query = _context.ProductColors.Include(p => p.product).Where(a => a.ProductId == id);
            return View(query);
        }

        public IActionResult CreateColor(int id)
        {
            ViewBag.product = id;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateColor(IEnumerable<ProductColor> Model, int id)
        {

            foreach (var item in Model)
            {
                var Color = new ProductColor()
                {
                    ProductId = id,
                    ColorName = item.ColorName
                };
                _context.ProductColors.Add(Color);
            }

            _context.SaveChanges();
            return RedirectToAction("ColorList", new { id = id });

        }


        [HttpGet]
        public async Task<IActionResult> DeleteColor(int id)
        {
            // var objFromDb = _dbContext.Categories.Where(i=>i.CategoryId==id);
            var objFromDb = await _context.ProductColors.FindAsync(id);

            if (objFromDb == null)
            {
                return Json(new { success = false, message = "Error while deleting." });
            }

            _context.ProductColors.Remove(objFromDb);
            _context.SaveChanges();

            return Json(new { success = true, message = "Delete successful." });

        }


        public async Task<IActionResult> Edit(int? id, int ProductId)
        {

            ViewBag.ProductId = ProductId;

            if (id == null)
            {
                return NotFound();
            }

            var properties = await _context.ProductColors.FindAsync(id);
            if (properties == null)
            {
                return NotFound();
            }

            return View(properties);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductColorId,ProductId,ColorName")] ProductColor productColor, int ProductId)
        {
            if (id != productColor.ProductColorId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(productColor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductColorExists(productColor.ProductColorId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("ColorList", new { id = ProductId });
            }

            return View(productColor);
        }

        


        

        private bool ProductColorExists(int id)
        {
            return _context.ProductColors.Any(e => e.ProductColorId == id);
        }
    }
}
