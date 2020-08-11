using DataLayer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TopKala3.Component
{


    [ViewComponent(Name = "InstantOffer")]
    public class InstantOffer : ViewComponent
    {

        private readonly TopKala3Contex _db;

        public InstantOffer(TopKala3Contex context)
        {
            _db = context;


        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var sliderAmazing = _db.Products.Include(p => p.properties).Include(c => c.Category)
                                            .Include(p => p.productPathImages)
                                            .OrderByDescending(i => i.ProductId)
                                            .Select(p => new AmazingSliderViewModel()
                                            {
                                                ProductID = p.ProductId,
                                                ProductName = p.Name,
                                                ProductPrice = p.Price,
                                                PriceReduction = p.PriceWithReduction,
                                                special=p.IsSpecial,

                                                ProductImage = p.productPathImages.FirstOrDefault().ImagePath,

                                                Name = p.Category.Name,
                                                Parent = p.Category.Parent.Name,
                                                Grand = p.Category.Parent.Parent.Name == null ? "Empty" : p.Category.Parent.Parent.Name


                                            }).Take(17);
            return View("/Views/Shared/Component/Offer.cshtml", sliderAmazing.ToList());
        }
    }


}
