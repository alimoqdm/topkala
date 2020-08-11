using DataLayer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace TopKala3.Component
{
    [ViewComponent(Name = "FavoriteComponent")]
    public class FavoriteComponent : ViewComponent
    {

        private readonly TopKala3Contex _db;

        public FavoriteComponent(TopKala3Contex context)
        {
            _db = context;

        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var userId  = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var product = _db.Products.Include(c => c.Category).Include(p => p.productPathImages).Where(a => a.UserLike == userId).Select(p => new ShowUserOrderViewModel()
            {
                Title = p.Name,
                Price = p.Price,
                ImageName = p.productPathImages.FirstOrDefault().ImagePath,
                ProductID = p.ProductId,
                PriceReduction=p.PriceWithReduction
               

            }).ToList();

            return View("/Views/Shared/Component/Favorite.cshtml", product);
        }

    }
}
