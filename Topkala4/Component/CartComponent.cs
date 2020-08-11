using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using DataLayer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace TopKala3.Component
{
    [ViewComponent(Name = "CartComponent")]
    public class CartComponent : ViewComponent
    {
        private TopKala3Contex _ctx;
        private readonly SignInManager<ApplicationUser> signInManager;
        private  List<ShowCartViewModel> _list;
        public CartComponent(TopKala3Contex ctx, SignInManager<ApplicationUser> signInManager)
        {
            _ctx = ctx;
            this.signInManager = signInManager;
            _list = new List<ShowCartViewModel>();
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
         

            if (User.Identity.IsAuthenticated)
            {
                string CurrentUserID = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

                var order = _ctx.Orders.SingleOrDefault(o => o.UserId == CurrentUserID && !o.IsFinaly);
                if (order != null)
                {
                    var details = _ctx.OrderDetails.Where(d => d.OrderId == order.OrderId).ToList();
                    foreach (var item in details)
                    {
                        var product = _ctx.Products.Include(a=>a.productPathImages).FirstOrDefault(f=>f.ProductId==item.ProductId);
                        
                        _list.Add(new ShowCartViewModel()
                        {
                            OrderDetailId = item.OrderDetailID,
                            Count = item.Count,
                            Title = product.Name,
                            ImageName = product.productPathImages.FirstOrDefault().ImagePath,
                            Color=item.Color,
                            Sum = item.Count * item.Price
                        });

                    }
                }

            }

            return View("/Views/Shared/Component/_ShowCart.cshtml", _list.ToList());
        }
    }
}
