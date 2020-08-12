using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using DataLayer;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace TopKala3.Areas.Admin.Controllers
{

    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class CreateProductController : Controller
    {
        private readonly TopKala3Contex _db;
        public CreateProductController(TopKala3Contex db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_db.Categories, "CategoryId", "Name");

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductId,Name,Price,PriceWithReduction,Reduction,IsSpecial,Seller,Tags,Review,Visit,CountSell,CreatDateTime,CategoryId,BrandId,EnglishName")] Product product)
        {
            if (ModelState.IsValid)
            {
                 
                product.CreatDateTime = DateTime.Now;
                // محاسبه درصد تخفیف
                int Precent = product.Reduction;
                product.PriceWithReduction = (Precent * product.Price) / 100;
                product.PriceWithReduction = product.Price - product.PriceWithReduction;
                _db.Add(product);
                await _db.SaveChangesAsync();
                ViewBag.id = product.ProductId;
                return RedirectToAction("Next", new { id = product.ProductId });
            }
            ViewData["CategoryId"] = new SelectList(_db.Categories, "CategoryId", "Name", product.CategoryId);
           
            return View(product);
        }

        public IActionResult Next(int id)
        {

            var product = _db.Products.Include(a=>a.Brands).SingleOrDefault(p=>p.ProductId==id);
            ViewData["CategoryId"] = new SelectList(_db.Categories, "CategoryId", "Name", product.CategoryId);
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("ProductId,Name,Brand,Price,PriceWithReduction,Reduction,IsSpecial,Seller,Tags,Review,Visit,CountSell,CreatDateTime,CategoryId,BrandId,EnglishName")] Product product)
        {
            if (ModelState.IsValid)
            {
               
                    _db.Update(product);
                    await _db.SaveChangesAsync();
   
                return RedirectToAction("Next", new { id = product.ProductId }); 
            }
            ViewData["CategoryId"] = new SelectList(_db.Categories, "CategoryId", "Name", product.CategoryId);
            return View("Next",product);
        }


        public IActionResult PropertyDetail(int id)
        {
            ViewBag.productid = id;
            var query = _db.Properties.Include(p => p.product).Where(a=>a.ProductId==id);
      
            return View("PropertyDetail", query.ToList());
        }

        
        [HttpGet]
        public IActionResult Property(int id)
        {
            ViewBag.product = id;
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Property( [Bind("PropertiesId,ProductId,Title,Value")] Properties properties)
        {
            if (ModelState.IsValid)
            {
           
                _db.Add(properties);
                await _db.SaveChangesAsync();
                return RedirectToAction("PropertyDetail", new { id = properties.ProductId });
            }
            return View(properties);
        }


        [HttpGet]
        public JsonResult GetBrands(int CountryId)
        {
            var Brands = new SelectList(_db.Brands.Where(c => c.CategoryId == CountryId), "BrandId", "Value");
            return Json(Brands);

        }

        [HttpGet]
        public JsonResult BrandsEdit(int CountryId)
        {
            var Brands = new SelectList(_db.Brands.Where(c => c.CategoryId == CountryId), "BrandId", "Value");
            return Json(Brands);

        }










    }
}