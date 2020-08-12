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
    public class ProductsController : Controller
    {
        private readonly TopKala3Contex _context;

        public ProductsController(TopKala3Contex context)
        {
            _context = context;
        }

        // GET: Admin/Products
        public async Task<IActionResult> Index()
        {
            var topKalaContext2 = _context.Products.Include(p => p.Category).Include(p => p.Brands);
            return View(await topKalaContext2.ToListAsync());
        }

        [HttpGet]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return Json(new { success = false, message = "Error while deleting." });
            }
            var color = await _context.ProductColors.Where(a => a.ProductId == product.ProductId).ToListAsync();
            foreach (var item in color)
            {
                _context.ProductColors.Remove(item);
            }
            var properties = await _context.Properties.Where(a => a.ProductId == product.ProductId).ToListAsync();
            foreach (var item in properties)
            {
                _context.Properties.Remove(item);
            }
            var image = await _context.ProductPathImages.Where(a => a.ProductId == product.ProductId).ToListAsync();
            foreach (var item in image)
            {
                _context.ProductPathImages.Remove(item);
            }
            var att = await _context.ProductAttributes.Where(a => a.ProductId == product.ProductId).ToListAsync();
            foreach (var item in att)
            {
                _context.ProductAttributes.Remove(item);
            }
            var com = await _context.Comments.Where(a => a.ProductId == product.ProductId).ToListAsync();
            foreach (var item in com)
            {
                _context.Comments.Remove(item);
            }
            var order = await _context.OrderDetails.Where(a => a.ProductId == product.ProductId).ToListAsync();
            foreach (var item in order)
            {
                _context.OrderDetails.Remove(item);
            }
            _context.SaveChanges();

            _context.Products.Remove(product);
           await _context.SaveChangesAsync();

            return Json(new { success = true, message = "Delete successful." });
        }

    }
}
