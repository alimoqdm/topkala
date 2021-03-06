﻿using DataLayer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TopKala3.Component
{

    [ViewComponent(Name = "ResponsiveSliderComponent")]
    public class ResponsiveSliderComponent : ViewComponent
    {

        private readonly TopKala3Contex _db;

        public ResponsiveSliderComponent(TopKala3Contex context)
        {
            _db = context;


        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var sliderAmazing = _db.Products.Include(p => p.properties).Include(c => c.Category)
                                            .Include(p => p.productPathImages)
                                            .Where(i => i.IsSpecial)
                                            .Select(p => new AmazingSliderViewModel()
                                            {
                                                ProductID = p.ProductId,
                                                ProductName = p.Name,
                                                ProductPrice = p.Price,
                                                PriceReduction = p.PriceWithReduction,

                                                ProductImage = p.productPathImages.FirstOrDefault().ImagePath,

                                                Name = p.Category.Name,
                                                Parent = p.Category.Parent.Name,
                                                Grand = p.Category.Parent.Parent.Name == null ? "Empty" : p.Category.Parent.Parent.Name


                                            }).Take(7);
            return View("/Views/Shared/Component/ResponsiveSlider.cshtml", sliderAmazing.ToList());
        }
    }


}
