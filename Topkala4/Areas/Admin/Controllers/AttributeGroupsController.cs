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
    public class AttributeGroupsController : Controller
    {
        private readonly TopKala3Contex _context;

        public AttributeGroupsController(TopKala3Contex context)
        {
            _context = context;
        }

        // GET: Admin/AttributeGroups
        public async Task<IActionResult> Index()
        {
            var topKala3Contex = _context.AttibuteGroups.Include(a => a.category);
            return View(await topKala3Contex.ToListAsync());
        }

        // GET: Admin/AttributeGroups/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories.Where(a=>a.Level==Level.Level2), "CategoryId", "Name");
            return View();
        }

        // POST: Admin/AttributeGroups/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AttributeGroupId,CategoryId,title")] AttributeGroup attributeGroup)
        {
            if (ModelState.IsValid)
            {
                _context.Add(attributeGroup);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories.Where(a => a.Level == Level.Level2), "CategoryId", "Name", attributeGroup.CategoryId);
            return View(attributeGroup);
        }

        // GET: Admin/AttributeGroups/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var attributeGroup = await _context.AttibuteGroups.FindAsync(id);
            if (attributeGroup == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories.Where(a => a.Level == Level.Level2), "CategoryId", "Name", attributeGroup.CategoryId);
            return View(attributeGroup);
        }

        // POST: Admin/AttributeGroups/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AttributeGroupId,CategoryId,title")] AttributeGroup attributeGroup)
        {
            if (id != attributeGroup.AttributeGroupId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(attributeGroup);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AttributeGroupExists(attributeGroup.AttributeGroupId))
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
            ViewData["CategoryId"] = new SelectList(_context.Categories.Where(a => a.Level == Level.Level2), "CategoryId", "Name", attributeGroup.CategoryId);
            return View(attributeGroup);
        }





        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
        
            var objFromDb = await _context.AttibuteGroups.FindAsync(id);

            if (objFromDb == null)
            {
                return Json(new { success = false, message = "Error while deleting." });
            }

            _context.AttibuteGroups.Remove(objFromDb);
           await  _context.SaveChangesAsync();


            return Json(new { success = true, message = "Delete successful." });

        }










        private bool AttributeGroupExists(int id)
        {
            return _context.AttibuteGroups.Any(e => e.AttributeGroupId == id);
        }
    }
}
