using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DataLayer;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Dto.Other;
using Dto.Payment;
using Microsoft.AspNetCore.Identity;

using ZarinPal.Class;

namespace TopKala3.Controllers
{
    [Authorize]
    public class OrdersController : Controller
    {
        private TopKala3Contex _ctx;
        private readonly IHttpContextAccessor _HttpContextAccessor;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly Payment _payment;

        public OrdersController(TopKala3Contex ctx, IHttpContextAccessor httpContextAccessor,
                                UserManager<ApplicationUser> userManager)
        {
            var expose = new Expose();
            _ctx = ctx;
            _HttpContextAccessor = httpContextAccessor;
            this.userManager = userManager;
            _payment = expose.CreatePayment();
        }


        
        public IActionResult AddToCart(ProductViewModel product)
        {
            var Reduction = product.Special;
            string CurrentUserID = _HttpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            Order order = _ctx.Orders.SingleOrDefault(o => o.UserId == CurrentUserID && !o.IsFinaly);
            if (order == null)
            {
                order = new Order()
                {
                    UserId = CurrentUserID,
                    CreateDate = DateTime.Now,
                    IsFinaly = false,
                    Sum = 0
                    
                };
                _ctx.Orders.Add(order);
                _ctx.SaveChanges();
                _ctx.OrderDetails.Add(new OrderDetail()
                {
                    OrderId = order.OrderId,
                    Count = 1,
                    Price =Reduction ? product.Price :product.PriceWithReduction,
                    ProductId = product.id,
                    Color = product.color

                });
                _ctx.SaveChanges();
            }
            else
            {
                var details = _ctx.OrderDetails.SingleOrDefault(d => d.OrderId == order.OrderId && d.ProductId == product.id);
                if (details == null)
                {
                    _ctx.OrderDetails.Add(new OrderDetail()
                    {
                        OrderId = order.OrderId,
                        Count = 1,
                        Price = Reduction ?  product.PriceWithReduction : product.Price ,
                        ProductId = product.id,
                        Color=product.color
                    });
                }
                else
                {
                    details.Count += 1;
                    _ctx.Update(details);
                }

                _ctx.SaveChanges();
            }
            UpdateSumOrder(order.OrderId);
            return RedirectToAction("ShowOrder");

        }




        public IActionResult ShowOrder()
        {
            string CurrentUserID = User.FindFirstValue(ClaimTypes.NameIdentifier);

            Order order = _ctx.Orders.SingleOrDefault(o => o.UserId == CurrentUserID && !o.IsFinaly);

            List<ShowOrderViewModel> _list = new List<ShowOrderViewModel>();
            if (order != null)
            {
                var details = _ctx.OrderDetails.Where(d => d.OrderId == order.OrderId).ToList();
                foreach (var item in details)
                {
                    var product = _ctx.Products.Include(a => a.productPathImages).FirstOrDefault(f => f.ProductId == item.ProductId);


                    _list.Add(new ShowOrderViewModel()
                    {
                        Count = item.Count,
                        ImageName = product.productPathImages.FirstOrDefault().ImagePath,
                        OrderDetailId = item.OrderDetailID,
                        Price = item.Price,
                        Sum = item.Count * item.Price,
                        Title = product.Name,
                        Color=item.Color
                    });

                }
            }

            return View(_list);
        }




        public IActionResult Delete(int id)
        {
            var orderDetail = _ctx.OrderDetails.Find(id);
            _ctx.Remove(orderDetail);
            _ctx.SaveChanges();
            return RedirectToAction("ShowOrder");
        }

        public IActionResult Command(int id, string command)
        {
            var orderDetail = _ctx.OrderDetails.Find(id);

            switch (command)
            {
                case "up":
                    {
                        orderDetail.Count += 1;
                        _ctx.Update(orderDetail);
                        break;
                    }
                case "down":
                    {
                        orderDetail.Count -= 1;
                        if (orderDetail.Count == 0)
                        {
                            _ctx.OrderDetails.Remove(orderDetail);
                        }
                        else
                        {
                            _ctx.Update(orderDetail);
                        }

                        break;
                    }
            }


            _ctx.SaveChanges();
            return RedirectToAction("ShowOrder");
        }





        public void UpdateSumOrder(int orderId)
        {
            var order = _ctx.Orders.Find(orderId);
            order.Sum = _ctx.OrderDetails.Where(o => o.OrderId == order.OrderId).Select(d => d.Count * d.Price).Sum();
            _ctx.Update(order);
            _ctx.SaveChanges();
        }


        public async Task<IActionResult> Payment()
        {
            string CurrentUserID = _HttpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = await userManager.FindByIdAsync(CurrentUserID);

            var order = _ctx.Orders.SingleOrDefault(o => !o.IsFinaly);
            if (order == null)
            {
                return NotFound();
            }

            var result = await _payment.Request(new DtoRequest()
            {
                Mobile = user.PhoneNumber,
                CallbackUrl = "http://topkalacore.ir/Home/OnlinePayment/" + order.OrderId,
                Description = "خرید",
                Email = user.Email,
                Amount = order.Sum,
                MerchantId = "XXXXXXXX-XXXX-XXXX-XXXX-XXXXXXXXXXXX"
            }, ZarinPal.Class.Payment.Mode.sandbox);

            var x = result.Status == 100;
            if (x)
            {
                return Redirect("https://sandbox.zarinpal.com/pg/StartPay/" + result.Authority);
            }
            else
            {
                return View("Shopping_Nobuy");
            }





            //var payment = new Payment(order.Sum);
            //var res =  payment.PaymentRequest($"پرداخت فاکتور شماره {order.OrderId}",
            //    "https://localhost:44313/Home/OnlinePayment/" + order.OrderId, user.Email, user.PhoneNumber);
            //var x = res.Result.Status == 100;
            //if (x)
            //{
            //    return Redirect("https://sandbox.zarinpal.com/pg/StartPay/" + res.Result.Authority);
            //}
            //else
            //{
            //    return View("Shopping_Nobuy");
            //}


        }
    }
}