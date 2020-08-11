using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataLayer;

namespace TopKala3.Component
{
    [ViewComponent(Name = "MenuViewComponent")]
    public class MenuViewComponent:ViewComponent
    {
        private readonly TopKala3Contex _db;

        public MenuViewComponent(TopKala3Contex context)
        {
            _db = context;


        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var menu = _db.Categories;
            return View("/Views/Shared/Component/Menu.cshtml", menu.ToList());
        }
    }
}
