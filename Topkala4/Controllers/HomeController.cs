using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using DataLayer;
using Dto.Payment;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MvcMovie.Models;
using ZarinPal.Class;

namespace TopKala3.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private TopKala3Contex _ctx;
        private readonly Payment _payment;

        public HomeController(ILogger<HomeController> logger, TopKala3Contex _ctx)
        {
            var expose = new Expose();
            _payment = expose.CreatePayment();
            _logger = logger;
            this._ctx = _ctx;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        public IActionResult test()
        {
            return View();
        }

        [HttpPost]
        public IActionResult test(string s)
        {
            ViewBag.s = s;
            return View("test2");
        }



        public async Task<IActionResult> OnlinePayment(int id,string authority, string status)
        {
            var order = _ctx.Orders.Find(id);
            var verification = await _payment.Verification(new DtoVerification
            {
                Amount = order.Sum,
                MerchantId = "XXXXXXXX-XXXX-XXXX-XXXX-XXXXXXXXXXXX",
                Authority = authority
            }, Payment.Mode.sandbox);

            if (verification.Status == 100)
            {
                order.IsFinaly = true;
                _ctx.Orders.Update(order);
                _ctx.SaveChanges();

                return View();
            }

            return View("FailPayment");











            //if (HttpContext.Request.Query["Status"] != "" &&
            //    HttpContext.Request.Query["Status"].ToString().ToLower() == "ok" &&
            //    HttpContext.Request.Query["Authority"] != "")
            //{
            //    string authority = HttpContext.Request.Query["Authority"].ToString();
            //    var order = _ctx.Orders.Find(id);
            //    var payment = new Payment(order.Sum);
            //    var res = payment.Verification(authority).Result;
            //    if (res.Status == 100)
            //    {
            //        order.IsFinaly = true;
            //        _ctx.Orders.Update(order);
            //        _ctx.SaveChanges();

            //        return View();
            //    }

            //}

            //return View("FailPayment");
        }
    }
}
