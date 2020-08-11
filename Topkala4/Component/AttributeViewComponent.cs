using DataLayer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TopKala3.Component
{

    [ViewComponent(Name = "AttributeViewComponent")]
    public class AttributeViewComponent:ViewComponent
    {
        private readonly TopKala3Contex _db;

        public AttributeViewComponent(TopKala3Contex context)
        {
            _db = context;


        }
        public async Task<IViewComponentResult> InvokeAsync(int id)
        {
            var catId = _db.Products.Include(a => a.Category).Where(a => a.ProductId == id).FirstOrDefault();

            var attributes = from p in _db.ProductAttributes.Where(a=>a.ProductId==id).ToList()
                             join a in _db.Attributes.Where(a=>a.attibuteGroup.CategoryId==catId.CategoryId || a.attibuteGroup.CategoryId==catId.Category.ParentId).ToList() on p.AttributeId equals a.AttributeId

                            select new AttViewModel
                            {
                                TITLE = a.title,
                                VALUE = p.Value,
                                ID =a.AttributeGroupId,
                                PID=p.ProductId
                            };

            var attgroup = _db.AttibuteGroups.Where(a => a.CategoryId == catId.CategoryId||a.CategoryId== catId.Category.ParentId).ToList();

            var attributess = from g in attgroup
                             join a in attributes.Where(a=>a.PID==id)
                             on g.AttributeGroupId equals a.ID
                             into AttributeGroup
                             select new AttGroupViewModel
                             {
                                  GroupName =g.title,
                                  Attr = AttributeGroup.ToList(),
                                  productid= id
                             };


            return View("/Views/Shared/Component/Attribute.cshtml", attributess.Where(a=>a.productid==id).ToList());
        }
    }
}
