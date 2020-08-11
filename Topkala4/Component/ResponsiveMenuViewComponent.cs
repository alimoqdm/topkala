using DataLayer;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TopKala3.Component
{
    [ViewComponent(Name = "ResponsiveMenuViewComponent")]
    public class ResponsiveMenuViewComponent : ViewComponent
    {
        private readonly TopKala3Contex _db;

        public ResponsiveMenuViewComponent(TopKala3Contex context)
        {
            _db = context;


        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var menu = _db.Categories;
            return View("/Views/Shared/Component/ResponsiveMenu.cshtml", menu.ToList());
        }
    }
}
