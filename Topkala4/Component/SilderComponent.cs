using DataLayer;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TopKala3.Component
{
    [ViewComponent(Name = "SilderComponent")]
    public class SilderComponent : ViewComponent
    {

        private readonly TopKala3Contex _db;

        public SilderComponent(TopKala3Contex context)
        {
            _db = context;


        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var slider = _db.SilderImage.Take(6);
            return View("/Views/Shared/Component/Slider.cshtml", slider.ToList());
        }
    }
   
    
}
