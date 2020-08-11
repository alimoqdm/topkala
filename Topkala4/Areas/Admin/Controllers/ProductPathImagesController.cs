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

namespace TopKala2.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductPathImagesController : Controller
    {
        private readonly TopKala3Contex _context;
       
        private readonly IHostingEnvironment _environment;
        public ProductPathImagesController(TopKala3Contex context, IHostingEnvironment _environment)
        {
            _context = context;
            this._environment = _environment;
        }
        public IActionResult ImageList(int id)
        {
            ViewBag.productid = id;
            var query = _context.ProductPathImages.Include(p => p.product).Where(a => a.ProductId == id);
            return View(query);
        }


        [HttpGet]
        public IActionResult CreateImage(int id)
        {
            ViewBag.product = id;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
     
        public async Task<IActionResult> CreateImage(IEnumerable<DataLayer.ImageUpVm> Model, int id)
        {
            if (ModelState.IsValid)
            {
                var uploadsRootFolder = Path.Combine(_environment.WebRootPath, "ProductImages");
                if (!Directory.Exists(uploadsRootFolder))
                {
                    Directory.CreateDirectory(uploadsRootFolder);
                }
                foreach (var item in Model)
                {
                    if(item.Photo!=null)
                    {
                        Guid guid = Guid.NewGuid();
                        var filePath = Path.Combine(uploadsRootFolder, item.Photo.FileName);
                        var name = Path.GetFileName(filePath);
                        var Image = new ProductPathImage()
                        {
                            ImagePath = guid + Path.GetExtension(name),
                            ProductId = id

                        };

                        _context.ProductPathImages.Add(Image);
                        using (var fileStream = new FileStream(Path.Combine(uploadsRootFolder, Image.ImagePath), FileMode.Create))
                        {
                            await item.Photo.CopyToAsync(fileStream).ConfigureAwait(false);
                        }
                    }
                }
                _context.SaveChanges();

                return RedirectToAction("ImageList", new { id = id });
            }
            return View(Model);
        }



        [HttpGet]
        public async Task<IActionResult> DeleteImage(int id)
        {
            var objFromDb = await _context.ProductPathImages.FindAsync(id);

            if (objFromDb == null)
            {
                return Json(new { success = false, message = "Error while deleting." });
            }

            var uploadsRootFolder = Path.Combine(_environment.WebRootPath, "ProductImages");
            if (objFromDb.ImagePath != null)
            {
                System.IO.File.Delete(Path.Combine(uploadsRootFolder, objFromDb.ImagePath));
            }
            _context.ProductPathImages.Remove(objFromDb);
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

            var properties = await _context.ProductPathImages.FindAsync(id);
            if (properties == null)
            {
                return NotFound();
            }

            return View(properties);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MyProperty,ProductId,ImagePath")] ProductPathImage productPathImage,IFormFile ImgsUp, int ProductId)
        {
            if (id != productPathImage.MyProperty)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (ImgsUp != null)
                {
                    var uploadsRootFolder = Path.Combine(_environment.WebRootPath, "ProductImages");
                    if (productPathImage.ImagePath != null)
                    {
                        System.IO.File.Delete(Path.Combine(uploadsRootFolder, productPathImage.ImagePath));
                    }



                    Guid guid = Guid.NewGuid();
                    var filePath = Path.Combine(uploadsRootFolder, ImgsUp.FileName);
                    var name = Path.GetFileName(filePath);
                    productPathImage.ImagePath = guid + Path.GetExtension(name);

                    using (var fileStream = new FileStream(Path.Combine(uploadsRootFolder, productPathImage.ImagePath), FileMode.Create))
                    {
                        await ImgsUp.CopyToAsync(fileStream).ConfigureAwait(false);
                    }

                }


                try
                {
                    _context.Update(productPathImage);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductPathImageExists(productPathImage.MyProperty))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("ImageList", new { id = ProductId });
            }

            return View(productPathImage);
        }


        private bool ProductPathImageExists(int id)
        {
            return _context.ProductPathImages.Any(e => e.MyProperty == id);
        }
    }
}
