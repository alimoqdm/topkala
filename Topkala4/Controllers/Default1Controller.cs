using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace TopKala4.Controllers
{
    public class Default1Controller : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}