using DataLayer;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TopKala3.Component
{
    [ViewComponent(Name = "ShowComment")]
    public class ShowComment : ViewComponent
    {

        private readonly TopKala3Contex _db;

        public ShowComment(TopKala3Contex context)
        {
            _db = context;


        }
        public async Task<IViewComponentResult> InvokeAsync(int id)
        {
            var Cm = _db.Comments.Where(a=>a.ProductId==id).OrderByDescending(a=>a.CreatDate);
            return View("/Views/Shared/Component/Comment.cshtml", Cm.ToList());
        }
    }
}
