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
    public class AttributeesController : Controller
    {
        private readonly TopKala3Contex _context;

        public AttributeesController(TopKala3Contex context)
        {
            _context = context;
        }

        // GET: Admin/Attributees
        public async Task<IActionResult> Index()
        {
            var topKala3Contex = _context.Attributes.Include(a => a.attibuteGroup);
            return View(await topKala3Contex.ToListAsync());
        }


        [HttpGet]
        public JsonResult GetAttrList(int CountryId)
        {
            var citylist = new SelectList(_context.AttibuteGroups.Where(c => c.CategoryId == CountryId), "AttributeGroupId", "title");
            return Json(citylist);

        }

        // GET: Admin/Attributees/Create
        public IActionResult Create()
        {
            ViewData["AttributeGroupId"] = new SelectList(_context.AttibuteGroups, "AttributeGroupId", "title");
            ViewData["CategoryId"] = new SelectList(_context.Categories.Where(a => a.Level == Level.Level2), "CategoryId", "Name");

            return View();
        }

        // POST: Admin/Attributees/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AttributeId,AttributeGroupId,title")] Attributee attributee)
        {
            if (ModelState.IsValid)
            {
                _context.Add(attributee);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AttributeGroupId"] = new SelectList(_context.AttibuteGroups, "AttributeGroupId", "title", attributee.AttributeGroupId);
            return View(attributee);
        }

        // GET: Admin/Attributees/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var attributee = await _context.Attributes.FindAsync(id);
            if (attributee == null)
            {
                return NotFound();
            }
            ViewData["AttributeGroupId"] = new SelectList(_context.AttibuteGroups, "AttributeGroupId", "title", attributee.AttributeGroupId);
            return View(attributee);
        }

        // POST: Admin/Attributees/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AttributeId,AttributeGroupId,title")] Attributee attributee)
        {
            if (id != attributee.AttributeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(attributee);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AttributeeExists(attributee.AttributeId))
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
            ViewData["AttributeGroupId"] = new SelectList(_context.AttibuteGroups, "AttributeGroupId", "title", attributee.AttributeGroupId);
            return View(attributee);
        }





        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {

            var objFromDb = await _context.Attributes.FindAsync(id);

            if (objFromDb == null)
            {
                return Json(new { success = false, message = "Error while deleting." });
            }

            _context.Attributes.Remove(objFromDb);
            await _context.SaveChangesAsync();


            return Json(new { success = true, message = "Delete successful." });

        }













        private bool AttributeeExists(int id)
        {
            return _context.Attributes.Any(e => e.AttributeId == id);
        }
    }
}
