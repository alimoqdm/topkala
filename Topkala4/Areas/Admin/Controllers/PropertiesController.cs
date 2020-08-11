using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DataLayer;
using Microsoft.AspNetCore.Authorization;

namespace TopKala2.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class PropertiesController : Controller
    {
        private readonly TopKala3Contex _context;

        public PropertiesController(TopKala3Contex context)
        {
            _context = context;
        }

        public IActionResult PropList(int id)
        {
            ViewBag.productid = id;
            var query = _context.Properties.Include(p => p.product).Where(a => a.ProductId == id);
            return View(query);
        }

        public IActionResult CreateProp(int id)
        {
            ViewBag.product = id;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateProp(IEnumerable<Properties> Model,int id)
        {
                    
            foreach(var item in Model)
            {
                var Prop = new Properties()
                {
                    ProductId = id,
                    Title = item.Title,
                    Value = item.Value
                };
               _context.Properties.Add(Prop);
            }

            _context.SaveChanges();
            return RedirectToAction("PropList", new { id = id });

        }


        [HttpGet]
        public async Task<IActionResult> DeleteProp(int id)
        {
            // var objFromDb = _dbContext.Categories.Where(i=>i.CategoryId==id);
            var objFromDb = await _context.Properties.FindAsync(id);

            if (objFromDb == null)
            {
                return Json(new { success = false, message = "Error while deleting." });
            }

            _context.Properties.Remove(objFromDb);
            _context.SaveChanges();

            return Json(new { success = true, message = "Delete successful." });

        }



        public async Task<IActionResult> Edit(int? id,int ProductId)
        {

            ViewBag.ProductId = ProductId;

            if (id == null)
            {
                return NotFound();
            }

            var properties = await _context.Properties.FindAsync(id);
            if (properties == null)
            {
                return NotFound();
            }
          
            return View(properties);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PropertiesId,ProductId,Title,Value")] Properties properties,int ProductId)
        {
            if (id != properties.PropertiesId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(properties);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PropertiesExists(properties.PropertiesId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("PropList", new { id = ProductId });
            }
     
            return View(properties);
        }




        private bool PropertiesExists(int id)
        {
            return _context.Properties.Any(e => e.PropertiesId == id);
        }
    }
}
