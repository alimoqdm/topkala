using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using DataLayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TopKala3.Utility;

namespace TopKala3.Areas.identity.Controllers
{
    [Area("identity")]
    public class AccountController : Controller
    {

        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly ILogger<AccountController> logger;
        IViewRenderService myRender;
        private readonly IHttpContextAccessor _HttpContextAccessor;
        private readonly TopKala3Contex _db;
        public AccountController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<AccountController> logger,
              IViewRenderService _myRender,
              IHttpContextAccessor httpContextAccessor,
               TopKala3Contex _db)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.logger = logger;
            myRender = _myRender;
            _HttpContextAccessor = httpContextAccessor;
            this._db = _db;
        }


        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }



        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                
                };

                var result = await userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    var token = await userManager.GenerateEmailConfirmationTokenAsync(user);

                    var confirmationLink =Url.Action("ConfirmEmail", "Account",
                                                              new { userId = user.Id, token = token }, Request.Scheme, "topkalacore.ir");

                    /* "http://www.topkalacore.ir/identity/Account/ConfirmEmail/" + user.Id + "/" + token;*/





                    var link = new EmailLink()
                    {
                        ConfirmLink = confirmationLink
                    };

                    //ساخت ویو برای ارسال ایمیل
                    string body = myRender.RenderToStringAsync("EmailView", link);

                    //ارسال ایمیل
                    SendEmail.Send(user.Email, "ایمیل فعالسازی", body);

                    ViewBag.UserName = user.UserName;
                    ViewBag.Email = user.Email;
                    return View("SuccessRegister");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(model);
        }


        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (userId == null || token == null)
            {
                return RedirectToAction("index", "home");
            }

            var user = await userManager.FindByIdAsync(userId);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"The User ID {userId} is invalid";
                return NotFound();
            }

            var result = await userManager.ConfirmEmailAsync(user, token);

            if (result.Succeeded)
            {
                ViewBag.userId = userId;
                return View();
            }

            ViewBag.ErrorTitle = "Email cannot be confirmed";
            return NotFound();
        }



        [AcceptVerbs("Get", "Post")]
        [AllowAnonymous]
        public async Task<IActionResult> IsEmailInUse(string email)
        {
            var user = await userManager.FindByEmailAsync(email);

            if (user == null)
            {
                return Json(true);
            }
            else
            {
                return Json($"Email {email} is already in use");
            }
        }


        [HttpGet]
        public IActionResult ComplteRegister(string userId)
        {
            ViewBag.userId = userId;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ComplteRegister(string userId,ComplteRegisterViewModel model)
        {
          
            if(ModelState.IsValid)
            {
                var user =await userManager.FindByIdAsync(userId);
                user.Name = model.Name;
                user.FamilyName = model.FamilyName;
                user.CardNumber = model.CardNumber;
                user.Newsletters = model.Newsletters;
                user.NationalCode = model.NationalCode;
                user.ForeignNational = model.ForeignNational;
                user.PhoneNumber = model.PhoneNumber;

               var result =  await userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                   
                    return View("IDK");
                }


            }


            return View(model);
        }







        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string returnUrl)
        {
            LoginViewModel model = new LoginViewModel
            {
                ReturnUrl = returnUrl,

            };

            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
      
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl)
        {
           

            if (ModelState.IsValid)
            {
                var user = await userManager.FindByEmailAsync(model.Email);

                if (user != null && !user.EmailConfirmed
                                  )
                {
                    ModelState.AddModelError(string.Empty, "Email not confirmed yet");
                    return View(model);
                }

                var result = await signInManager.PasswordSignInAsync(model.Email, model.Password,
                                        model.RememberMe, false);

               

                if (result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return Redirect("/");
                    }
                }

            
                ModelState.AddModelError(string.Empty, "Invalid Login Attempt");
            }

            return View(model);
        }







     
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return Redirect("/");
        }




        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByEmailAsync(model.Email);

                if (user != null && await userManager.IsEmailConfirmedAsync(user))
                {
                    var token = await userManager.GeneratePasswordResetTokenAsync(user);

                    var passwordResetLink = Url.Action("ResetPassword", "Account",
                            new { email = model.Email, token = token }, Request.Scheme, "topkalacore.ir");

                    var link = new EmailLink()
                    {
                        ConfirmLink = passwordResetLink
                    };

                    //ساخت ویو برای ارسال ایمیل
                    string body = myRender.RenderToStringAsync("EmailViewPass", link);

                    //ارسال ایمیل
                    SendEmail.Send(user.Email, "ایمیل تغییر رمز", body);

                    return View("ForgotPasswordConfirmation");
                }

                 
            }

            return View(model);
        }







        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPassword(string token, string email)
        {
            if (token == null || email == null)
            {
                ModelState.AddModelError("", "Invalid password reset token");
            }
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByEmailAsync(model.Email);

                if (user != null)
                {
                    var result = await userManager.ResetPasswordAsync(user, model.Token, model.Password);
                    if (result.Succeeded)
                    {
                        if (await userManager.IsLockedOutAsync(user))
                        {
                            await userManager.SetLockoutEndDateAsync(user, DateTimeOffset.UtcNow);
                        }

                        return View("ResetPasswordConfirmation");
                    }

                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    return View(model);
                }

              
            }

            return View(model);
        }



        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var userId = _HttpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = await userManager.FindByIdAsync(userId);

            return View(user);
        }


        [HttpGet]
        public async Task<IActionResult> PersonalData()
        {
            var userId = _HttpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = await userManager.FindByIdAsync(userId);

            return View(user);
        }

        [HttpGet]
        public async Task<IActionResult> ProfileOrder()
        {
            var userId = _HttpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = await userManager.FindByIdAsync(userId);
            List<ShowUserOrderViewModel> _list = new List<ShowUserOrderViewModel>();
            ViewBag.Name = user.Name;
            ViewBag.Family = user.FamilyName;

            var order = _db.Orders.Include(a => a.OrderDetails).Where(a => a.UserId == userId && !a.IsFinaly).FirstOrDefault();
            if(order!=null)
            {

            
            var orderDetail = order.OrderDetails.ToList();

            if(orderDetail!=null)
            {
                foreach (var item in orderDetail)
                {
                    var product = _db.Products.Include(a => a.productPathImages).Include(a => a.Category).FirstOrDefault(f => f.ProductId == item.ProductId);


                        _list.Add(new ShowUserOrderViewModel()
                        {

                            ImageName = product.productPathImages.FirstOrDefault().ImagePath,

                            Price = item.Price,
                            OrderDetailId = item.OrderDetailID,
                            Title = product.Name,
                            color = item.Color,
                            ProductID = product.ProductId,
                            Name = product.Category.Name,
                            PriceReduction = product.PriceWithReduction

                        }) ;

                }
            }
            }

            return View(_list.ToList());
        }


        [HttpGet]
        public async Task<IActionResult> favorites()
        {
            var userId = _HttpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = await userManager.FindByIdAsync(userId);
          
            ViewBag.Name = user.Name;
            ViewBag.Family = user.FamilyName;
            var product = _db.Products.Include(c=>c.Category).Include(p=>p.productPathImages).Where(a => a.UserLike == userId).Select(p=> new ShowUserOrderViewModel() { 
            Title =p.Name,
            Price=p.Price,
            ImageName=p.productPathImages.FirstOrDefault().ImagePath,
            ProductID =p.ProductId,
            Name=p.Category.Name,
            Parent=p.Category.Parent.Name,
            Grand = p.Category.Parent.Parent.Name==null ? "Empty" : p.Category.Parent.Parent.Name,
            PriceReduction =p.PriceWithReduction

            }).ToList();

            return View(product);
        }








        [HttpGet]
        public async Task<IActionResult> ChangePassword()
        {
            var user = await userManager.GetUserAsync(User);

            var userHasPassword = await userManager.HasPasswordAsync(user);

            if (!userHasPassword)
            {
                return RedirectToAction("AddPassword");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.GetUserAsync(User);
                if (user == null)
                {
                    return RedirectToAction("Login");
                }

                var result = await userManager.ChangePasswordAsync(user,
                    model.CurrentPassword, model.NewPassword);

                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return View();
                }

                await signInManager.RefreshSignInAsync(user);
                return View("ChangePasswordConfirmation");
            }

            return View(model);
        }


        [HttpGet]
        public async Task<IActionResult> EditProfile()
        {
            var userId = _HttpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = await userManager.FindByIdAsync(userId);

            var model = new EditProFileViewModel()
            {
                CardNumber = user.CardNumber,
                Newsletters = user.Newsletters,
                FamilyName = user.FamilyName,
                ForeignNational = user.ForeignNational,
                Name = user.Name,
                NationalCode = user.NationalCode,
                PhoneNumber =user.PhoneNumber,
                ID= userId
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditProfile(EditProFileViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByIdAsync(model.ID);
                user.Name = model.Name;
                user.FamilyName = model.FamilyName;
                user.CardNumber = model.CardNumber;
                user.Newsletters = model.Newsletters;
                user.NationalCode = model.NationalCode;
                user.ForeignNational = model.ForeignNational;
                user.PhoneNumber = model.PhoneNumber;

                var result = await userManager.UpdateAsync(user);

                if (result.Succeeded)
                {

                    return View("Profile",user);
                }


            }


            return View(model);
        }



    }
}