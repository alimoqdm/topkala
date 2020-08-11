using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DataLayer;
using Microsoft.EntityFrameworkCore;

namespace TopKala3.Controllers
{
    public class SearchController : Controller
    {
        private TopKala3Contex _db;
        public SearchController(TopKala3Contex _db)
        {
            this._db = _db;
        }


        [Route("/Search/{id}")]
        public async Task<IActionResult> SearchProduct(int id,string SortOrder,string Search,string SerachInProduct, int pageid=1)
        {

            int Count1 = 0;
            int skip = (pageid - 1) * 12;
           
            ViewBag.Id = id;
           var product = _db.Products.Include(c=>c.Category).Include(a=>a.productPathImages).Where(a => a.CategoryId == id).Select(g => new SearchViewModel()
            {
               ProductID =g.ProductId,
                ProductName =g.Name,
                 ProductPrice=g.Price,
                 ProductImage =g.productPathImages.FirstOrDefault().ImagePath,
                 Visit=g.Visit,
                 Date=g.CreatDateTime,       
                Categoryies = g.Category.Categories.ToList(),
                CategoryName=g.Category.Name,
               CategoryParentName = g.Category.Parent.Name,
               CategoryGrandParentName = g.Category.Parent.Parent.Name==null ? "Empty": g.Category.Parent.Parent.Name,
               special=g.IsSpecial,
               PriceReduction=g.PriceWithReduction,
               CategoryId=g.CategoryId

           });

       

            if (!String.IsNullOrEmpty(Search))
            {
                var pro =await  _db.Products.Include(a=>a.Category).Where(a=>a.Name.ToLower().Contains(Search)||a.Category.Name.ToLower().Contains(Search)).Select(g => new SearchViewModel()
                {
                    ProductID = g.ProductId,
                    ProductName = g.Name,
                    ProductPrice = g.Price,
                    ProductImage = g.productPathImages.FirstOrDefault().ImagePath,
                    Visit = g.Visit,
                    Date = g.CreatDateTime,
                    CategoryName = g.Category.Name,
                    CategoryParentName = g.Category.Parent.Name,
                    CategoryGrandParentName = g.Category.Parent.Parent.Name == null ? "Empty" : g.Category.Parent.Parent.Name,
                    special = g.IsSpecial,
                    PriceReduction = g.PriceWithReduction,
                    CategoryId = g.CategoryId

                }).ToListAsync();
                if(!pro.Any())
                {
                    return View("NonProduct");
                }

                else
                {
                    ViewBag.Name = pro.FirstOrDefault().CategoryName;
                    ViewBag.Count = pro.Count();
                    ViewBag.Idd = pro.FirstOrDefault().CategoryId;
                    return View(pro.OrderBy(a => a.ProductID).Skip(skip).Take(12));
                }
               
               
            }

            if (!String.IsNullOrEmpty(SerachInProduct))
            {
                var pro = product.Where(a => a.ProductName.ToLower().Contains(SerachInProduct));


                if (!pro.Any())
                {
                    return View("NonProduct");
                }

                else
                {
                    ViewBag.Name = pro.FirstOrDefault().CategoryName;
                    ViewBag.Count = pro.Count();
                    ViewBag.Idd = pro.FirstOrDefault().CategoryId;
                    return View(await pro.OrderBy(a => a.ProductID).Skip(skip).Take(12).ToListAsync());
                }
            }


                ViewBag.Count = product.Count();
            switch (SortOrder)
            {
                case "Normal":
                    
                    product = product.OrderBy(a => a.ProductID);
                    ViewBag.Name = product.FirstOrDefault().CategoryName;
                    ViewBag.Idd = product.FirstOrDefault().CategoryId;
                     Count1 = product.Count();
                    ViewBag.PageID = pageid;
                    ViewBag.PageCount = Count1 / 12;

                    return PartialView(await product.Skip(skip).Take(12).ToListAsync());
                    

                case "Visit":
                    
                    product = product.OrderByDescending(a => a.Visit);
                    ViewBag.Name = product.FirstOrDefault().CategoryName;
                    ViewBag.Idd = product.FirstOrDefault().CategoryId;
                     Count1 = product.Count();
                    ViewBag.PageID = pageid;
                    ViewBag.PageCount = Count1 / 12;
                    return PartialView(await product.Skip(skip).Take(12).ToListAsync());

                case "New":
                  
                    product = product.OrderByDescending(a => a.Date);
                    ViewBag.Name = product.FirstOrDefault().CategoryName;
                    ViewBag.Idd = product.FirstOrDefault().CategoryId;
                     Count1 = product.Count();
                    ViewBag.PageID = pageid;
                    ViewBag.PageCount = Count1 / 12;
                    return PartialView(await product.Skip(skip).Take(12).ToListAsync());

                case "Low":
                   
                    product = product.OrderBy(a => a.ProductPrice);
                    ViewBag.Name = product.FirstOrDefault().CategoryName;
                    ViewBag.Idd = product.FirstOrDefault().CategoryId;
                     Count1 = product.Count();
                    ViewBag.PageID = pageid;
                    ViewBag.PageCount = Count1 / 12;
                    return PartialView(await product.Skip(skip).Take(12).ToListAsync());

                case "High":
                    
                    product = product.OrderByDescending(a => a.ProductPrice);
                    ViewBag.Name = product.FirstOrDefault().CategoryName;
                    ViewBag.Idd = product.FirstOrDefault().CategoryId;
                     Count1 = product.Count();
                    ViewBag.PageID = pageid;
                    ViewBag.PageCount = Count1 / 12;
                    return PartialView(await product.Skip(skip).Take(12).ToListAsync());


            }

                                                                                                            
            ViewBag.Name = product.FirstOrDefault().CategoryName;
            ViewBag.Idd = product.FirstOrDefault().CategoryId;
            int Count = product.Count();
            ViewBag.PageID = pageid;
            ViewBag.PageCount = Count / 12;
            return View(await product.Skip(skip).Take(12).ToListAsync());
        }





        [Route("/Filter/{id}")]
        public async Task<IActionResult> Filter(int id, string SortOrder, string Search, string SerachInProduct, string CategoryName, int pageid = 1)
        {

            int Count1 = 0;
            int skip = (pageid - 1) * 12;

            ViewBag.Id = id;
            var product = _db.Products.Include(c => c.Category).Include(a => a.productPathImages).Where(a => a.CategoryId == id).Select(g => new SearchViewModel()
            {
                ProductID = g.ProductId,
                ProductName = g.Name,
                ProductPrice = g.Price,
                ProductImage = g.productPathImages.FirstOrDefault().ImagePath,
                Visit = g.Visit,
                Date = g.CreatDateTime,
                Categoryies = g.Category.Categories.ToList(),
                CategoryName = g.Category.Name,
                CategoryParentName = g.Category.Parent.Name,
                CategoryGrandParentName = g.Category.Parent.Parent.Name == null ? "Empty" : g.Category.Parent.Parent.Name,
                special = g.IsSpecial,
                PriceReduction = g.PriceWithReduction,
                CategoryId = g.CategoryId

            });



            if (!String.IsNullOrEmpty(Search))
            {
                var pro = await _db.Products.Include(a => a.Category).Where(a => a.Name.ToLower().Contains(Search) || a.Category.Name.ToLower().Contains(Search)).Select(g => new SearchViewModel()
                {
                    ProductID = g.ProductId,
                    ProductName = g.Name,
                    ProductPrice = g.Price,
                    ProductImage = g.productPathImages.FirstOrDefault().ImagePath,
                    Visit = g.Visit,
                    Date = g.CreatDateTime,
                    CategoryName = g.Category.Name,
                    CategoryParentName = g.Category.Parent.Name,
                    CategoryGrandParentName = g.Category.Parent.Parent.Name == null ? "Empty" : g.Category.Parent.Parent.Name,
                    special = g.IsSpecial,
                    PriceReduction = g.PriceWithReduction,
                    CategoryId = g.CategoryId

                }).ToListAsync();
                if (!pro.Any())
                {
                    return View("NonProduct");
                }

                else
                {
                    ViewBag.Name = pro.FirstOrDefault().CategoryName;
                    ViewBag.Count = pro.Count();
                    ViewBag.Idd = pro.FirstOrDefault().CategoryId;
                    return View(pro.OrderBy(a => a.ProductID).Skip(skip).Take(12));
                }


            }

            if (!String.IsNullOrEmpty(SerachInProduct))
            {
                var pro = product.Where(a => a.ProductName.ToLower().Contains(SerachInProduct));


                if (!pro.Any())
                {
                    return View("NonProduct");
                }

                else
                {
                    ViewBag.Name = pro.FirstOrDefault().CategoryName;
                    ViewBag.Count = pro.Count();
                    ViewBag.Idd = pro.FirstOrDefault().CategoryId;
                    return View(await pro.OrderBy(a => a.ProductID).Skip(skip).Take(12).ToListAsync());
                }
            }


            ViewBag.Count = product.Count();
            switch (SortOrder)
            {
                case "Normal":

                    product = product.OrderBy(a => a.ProductID);
                    ViewBag.Name = product.FirstOrDefault().CategoryName;
                    ViewBag.Idd = product.FirstOrDefault().CategoryId;
                    Count1 = product.Count();
                    ViewBag.PageID = pageid;
                    ViewBag.PageCount = Count1 / 12;

                    return PartialView(await product.Skip(skip).Take(12).ToListAsync());


                case "Visit":

                    product = product.OrderByDescending(a => a.Visit);
                    ViewBag.Name = product.FirstOrDefault().CategoryName;
                    ViewBag.Idd = product.FirstOrDefault().CategoryId;
                    Count1 = product.Count();
                    ViewBag.PageID = pageid;
                    ViewBag.PageCount = Count1 / 12;
                    return PartialView(await product.Skip(skip).Take(12).ToListAsync());

                case "New":

                    product = product.OrderByDescending(a => a.Date);
                    ViewBag.Name = product.FirstOrDefault().CategoryName;
                    ViewBag.Idd = product.FirstOrDefault().CategoryId;
                    Count1 = product.Count();
                    ViewBag.PageID = pageid;
                    ViewBag.PageCount = Count1 / 12;
                    return PartialView(await product.Skip(skip).Take(12).ToListAsync());

                case "Low":

                    product = product.OrderBy(a => a.ProductPrice);
                    ViewBag.Name = product.FirstOrDefault().CategoryName;
                    ViewBag.Idd = product.FirstOrDefault().CategoryId;
                    Count1 = product.Count();
                    ViewBag.PageID = pageid;
                    ViewBag.PageCount = Count1 / 12;
                    return PartialView(await product.Skip(skip).Take(12).ToListAsync());

                case "High":

                    product = product.OrderByDescending(a => a.ProductPrice);
                    ViewBag.Name = product.FirstOrDefault().CategoryName;
                    ViewBag.Idd = product.FirstOrDefault().CategoryId;
                    Count1 = product.Count();
                    ViewBag.PageID = pageid;
                    ViewBag.PageCount = Count1 / 12;
                    return PartialView(await product.Skip(skip).Take(12).ToListAsync());


            }


            ViewBag.Name = product.FirstOrDefault().CategoryName;
            ViewBag.Idd = product.FirstOrDefault().CategoryId;
            int Count = product.Count();
            ViewBag.PageID = pageid;
            ViewBag.PageCount = Count / 12;
            return PartialView(await product.Skip(skip).Take(12).ToListAsync());
        }


        [Route("/NoProduct")]
        public IActionResult NoProduct()
        {
            return PartialView();
        }
        [Route("/NoSendProduct")]
        public IActionResult NoSendProduct()
        {
            return PartialView();
        }









    }
}
