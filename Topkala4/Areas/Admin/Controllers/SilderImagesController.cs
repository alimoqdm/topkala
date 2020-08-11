using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DataLayer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Authorization;

namespace TopKala3.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles ="Admin")]
    public class SilderImagesController : Controller
    {
        private readonly TopKala3Contex _context;
        private readonly IHostingEnvironment _environment;

        public SilderImagesController(TopKala3Contex context, IHostingEnvironment _environment)
        {
            _context = context;
            this._environment = _environment;
        }

        // GET: Admin/SilderImages
        public async Task<IActionResult> Index()
        {
            var topKala3Contex = _context.SilderImage.Include(s => s.category);
            return View(await topKala3Contex.ToListAsync());
        }


        [HttpGet]
        public async Task<IActionResult> DeleteImage(int id)
        {
            var objFromDb = await _context.SilderImage.FindAsync(id);

            if (objFromDb == null)
            {
                return Json(new { success = false, message = "Error while deleting." });
            }

            _context.SilderImage.Remove(objFromDb);
          await  _context.SaveChangesAsync();


            return Json(new { success = true, message = "Delete successful." });
        }



        // GET: Admin/SilderImages/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "Name");
            return View();
        }

        // POST: Admin/SilderImages/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SilderId,CategoryId,Image")] SilderImage silderImage, IFormFile imgsUp)
        {

            if (ModelState.IsValid)
            {

                if (imgsUp != null)
                {
                    var uploadsRootFolder = Path.Combine(_environment.WebRootPath, "SliderImages");
                    if (!Directory.Exists(uploadsRootFolder))
                    {
                        Directory.CreateDirectory(uploadsRootFolder);
                    }
                    Guid guid = Guid.NewGuid();

                    var filePath = Path.Combine(uploadsRootFolder, imgsUp.FileName);
                    var name = Path.GetFileName(filePath);
                    silderImage.Image = guid + Path.GetExtension(name);

                    using (var fileStream = new FileStream(Path.Combine(uploadsRootFolder, silderImage.Image), FileMode.Create))
                    {
                        await imgsUp.CopyToAsync(fileStream).ConfigureAwait(false);
                    }

                   
                }
                _context.Add(silderImage);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));

            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "Name", silderImage.CategoryId);
            return View(silderImage);
        }
        // GET: Admin/SilderImages/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var silderImage = await _context.SilderImage.FindAsync(id);
            if (silderImage == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "Name", silderImage.CategoryId);
            return View(silderImage);
        }

        // POST: Admin/SilderImages/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SilderId,CategoryId,Image")] SilderImage silderImage, IFormFile imgsUp)
        {
            if (id != silderImage.SilderId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {



                if (imgsUp != null)
                {
                    var uploadsRootFolder = Path.Combine(_environment.WebRootPath, "SliderImages");
                    if (silderImage.Image != null)
                    {
                        System.IO.File.Delete(Path.Combine(uploadsRootFolder, silderImage.Image));
                    }



                    Guid guid = Guid.NewGuid();
                    var filePath = Path.Combine(uploadsRootFolder, imgsUp.FileName);
                    var name = Path.GetFileName(filePath);
                    silderImage.Image = guid + Path.GetExtension(name);

                    using (var fileStream = new FileStream(Path.Combine(uploadsRootFolder, silderImage.Image), FileMode.Create))
                    {
                        await imgsUp.CopyToAsync(fileStream).ConfigureAwait(false);
                    }

                }




                try
                {
                    _context.Update(silderImage);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SilderImageExists(silderImage.SilderId))
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
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "Name", silderImage.CategoryId);
            return View(silderImage);
        }

        // GET: Admin/SilderImages/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var silderImage = await _context.SilderImage
                .Include(s => s.category)
                .FirstOrDefaultAsync(m => m.SilderId == id);
            if (silderImage == null)
            {
                return NotFound();
            }

            return View(silderImage);
        }

        // POST: Admin/SilderImages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var silderImage = await _context.SilderImage.FindAsync(id);
            var uploadsRootFolder = Path.Combine(_environment.WebRootPath, "SliderImages");
            if (silderImage.Image != null)
            {
                System.IO.File.Delete(Path.Combine(uploadsRootFolder, silderImage.Image));
            }
            _context.SilderImage.Remove(silderImage);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SilderImageExists(int id)
        {
            return _context.SilderImage.Any(e => e.SilderId == id);
        }
    }
}
