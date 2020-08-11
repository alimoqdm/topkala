using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DataLayer;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace TopKala3.Controllers
{
    public class ProductController : Controller
    {
        private readonly TopKala3Contex _db;
        private readonly UserManager<ApplicationUser> userManager;
        public ProductController(TopKala3Contex _db, UserManager<ApplicationUser> userManager)
        {
            this._db = _db;
            this.userManager = userManager;
        }


        [Route("/Product/{id},{Name},{Parent},{Grand}")]
        public async Task<IActionResult> Product(int id,string Name,string Parent,string Grand)
        {
            ViewBag.Name = Name;
            ViewBag.Parent = Parent;
            ViewBag.Grand = Grand;

            var Product1 = await _db.Products.Include(a => a.productPathImages).Include(a => a.productColors)
              
                .Include(a => a.properties)
                .Include(a => a.productAttributes)
                .Include(a => a.Category)
                .Include(a => a.comments)
                .Include(a => a.productAttributes)

                .ThenInclude(a => a.attribute)
                .ThenInclude(a => a.attibuteGroup)
                
                .FirstOrDefaultAsync(a => a.ProductId == id);
            Product1.Visit += 1;
            _db.Products.Update(Product1);
            await _db.SaveChangesAsync();

            var images = Product1.productPathImages.ToList();
            var Colors = Product1.productColors.ToList();
            var properties = Product1.properties.ToList();
            var attribute = Product1.productAttributes.ToList();
            var frist = Product1.productPathImages.FirstOrDefault().ImagePath;

            var ProductViewModel = new ProductViewModel()
            {
                id = Product1.ProductId,
                Name = Product1.Name,
                Price = Product1.Price,
                Review = Product1.Review,
                Reduction = Product1.Reduction,
                Category = Product1.Category.Name,
                Brand = Product1.Brand,
                FristImage = frist,
                Colors = Colors,
                Images = images,
                Propertiess = properties,
                Attributes = attribute,
                CommentCount = Product1.comments.Count(),
                Special= Product1.IsSpecial,
                PriceWithReduction=Product1.PriceWithReduction,
                EngName=Product1.EnglishName

            };

            return View(ProductViewModel);
        }


        [Authorize]
        public async Task<IActionResult> AddComment(int id, string comment)
        {
            string CurrentUserID = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await userManager.FindByIdAsync(CurrentUserID);

            Comment NewCm = new Comment()
            {
                CreatDate = DateTime.Now,
                Text = comment,
                Name = user.Name,
                ProductId = id


            };

            _db.Comments.Add(NewCm);
            await _db.SaveChangesAsync();

            return ViewComponent("ShowComment", new { id = id });
        }


        [Route("/Product/CountCm/{id}")]
        public string CountCm(int id)
        {
            var pro = _db.Products.Include(a => a.comments).FirstOrDefault(a => a.ProductId == id);
            int Count = pro.comments.Count();
            return Count.ToString("# نظر");
        }


        public async Task<IActionResult> UserProduct(int id,string Name,string Color)
        {
            ViewBag.Name = Name;
            ViewBag.Color = Color;
            var Product1 = await _db.Products.Include(a => a.productPathImages).Include(a => a.productColors)
                .Include(a => a.properties)
                .Include(a => a.productAttributes)
                .Include(a => a.Category)
                .Include(a => a.comments)
                .Include(a => a.productAttributes)

                .ThenInclude(a => a.attribute)
                .ThenInclude(a => a.attibuteGroup)

                .FirstOrDefaultAsync(a => a.ProductId == id);
          
            var images = Product1.productPathImages.ToList();
            var Colors = Product1.productColors.ToList();
            var properties = Product1.properties.ToList();
            var attribute = Product1.productAttributes.ToList();
            var frist = Product1.productPathImages.FirstOrDefault().ImagePath;

            var ProductViewModel = new ProductViewModel()
            {
                id = Product1.ProductId,
                Name = Product1.Name,
                Price = Product1.Price,
                Review = Product1.Review,
                Reduction = Product1.Reduction,
                Category = Product1.Category.Name,
                Brand = Product1.Brand,
                FristImage = frist,
              
                Images = images,
                Propertiess = properties,
                Attributes = attribute,
                CommentCount = Product1.comments.Count(),
                EngName = Product1.EnglishName

            };

            return View(ProductViewModel);
        }

        [Route("/UserLikeProduct/{id}/{like}")]
        public IActionResult UserLikeProduct(int id,string like)
        {
            string CurrentUserID = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var product = _db.Products.Where(a => a.ProductId == id).FirstOrDefault();

            if(like=="true")
            {
                product.UserLike = CurrentUserID;
                _db.Products.Update(product);
                _db.SaveChanges();
            }
            else
            {
                product.UserLike = null;
                _db.Products.Update(product);
                _db.SaveChanges();
            }

            return Redirect("/identity/Account/favorites");
        }
    }
}