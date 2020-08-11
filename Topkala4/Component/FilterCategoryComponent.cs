using DataLayer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TopKala3.Component
{
    [ViewComponent(Name = "FilterCategoryComponent")]
    public class FilterCategoryComponent : ViewComponent
    {

        private readonly TopKala3Contex _db;

        public FilterCategoryComponent(TopKala3Contex context)
        {
            _db = context;

        }

        public async Task<IViewComponentResult> InvokeAsync(int id)
        {
            var pro = _db.Products.Include(a=>a.Category).Where(a => a.Category.CategoryId == id).FirstOrDefault();
            ViewBag.Filter = pro.Category.Name;
            var Filter = pro.Category.Categories;
            return View("/Views/Shared/Component/ww.cshtml", Filter.ToList());
        }
    }
}
