using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataLayer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace TopKala3.Areas.Admin.Controllers
{

    [Area("Admin")]
    public class CreateProductAttributeController : Controller
    {
        private readonly TopKala3Contex _db;
        public CreateProductAttributeController(TopKala3Contex db)
        {
            _db = db;
        }


        [HttpGet]
        public IActionResult Attribute(int id)
        {
            ViewBag.x = id;
            var x = _db.Products.Include(c=>c.Category).Where(a => a.ProductId == id).FirstOrDefault();
             
          

            if (x != null)
            {
            var title = _db.Attributes.Where(a => a.attibuteGroup.CategoryId == x.Category.ParentId|| a.attibuteGroup.CategoryId == x.Category.CategoryId).Select(a => a.title).ToList();
            var attributeid = _db.Attributes.Where(a => a.attibuteGroup.CategoryId == x.Category.ParentId || a.attibuteGroup.CategoryId == x.Category.CategoryId).Select(a => a.AttributeId).ToList();

                var attribute = new AttributeViewModel()
                    {
                        ProductID = ViewBag.x,
                        Att = title,
                        AttributeID = attributeid,
                        Val=null
                    };
                  
                
                return View("Index", attribute);

            }

            else
            {
                return NotFound();
            }

        }

        [HttpPost]
        public async Task<IActionResult> Attribute([FromForm] AttributeViewModel attributes)
        {
            if (ModelState.IsValid)
            {
             
                foreach (var item in attributes.Val.Zip(attributes.AttributeID, (a, b) => new { A = a, B = b }))
                {
                    if(item.A!=null)
                    {
                        var att = new ProductAttribute()
                        {
                            ProductId = attributes.ProductID,
                            Value = item.A,
                            AttributeId = item.B

                        };
                    
                   
                    _db.Add(att);
                    }
                }
             
                await _db.SaveChangesAsync();
                return View("View",_db.ProductAttributes.ToList());
            }
          
            return View(attributes);
        }


 

    }
}