using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DataLayer;
using Microsoft.AspNetCore.Authorization;

namespace TopKala2.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class CategoriesController : Controller
    {
        private readonly ICategoryRepository _repository;
        private readonly TopKala3Contex _dbContext;
        public CategoriesController(ICategoryRepository context,TopKala3Contex _dbContext)
        {
            _repository = context;
            this._dbContext = _dbContext;
        }

        // GET: Admin/Categories
        public async Task<IActionResult> Index()
        {

            return View(await _repository.Get(a => a.Parent));
        }

        // GET: Admin/Categories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _repository.GetDetalis(id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // GET: Admin/Categories/Create
        public async Task<IActionResult> Create()
        {
            ViewData["ParentId"] = new SelectList(await _repository.Get(), "CategoryId", "Name");
            return View();
        }

        // POST: Admin/Categories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CategoryId,Name,Level,ParentId")] Category category)
        {
            if (ModelState.IsValid)
            {
              await _repository.Create(category);
               
                return RedirectToAction(nameof(Index));
            }
            ViewData["ParentId"] = new SelectList(await _repository.Get(), "CategoryId", "Name", category.ParentId);
            return View(category);
        }

        // GET: Admin/Categories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _repository.GetById(id);
            if (category == null)
            {
                return NotFound();
            }
            ViewData["ParentId"] = new SelectList(await _repository.Get(), "CategoryId", "Name", category.ParentId);
            return View(category);
        }

        // POST: Admin/Categories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CategoryId,Name,Level,ParentId")] Category category)
        {
            if (id != category.CategoryId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                  await  _repository.Update(category);
                
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_repository.CategoryExists(category.CategoryId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ParentId"] = new SelectList(await _repository.Get(), "CategoryId", "Name", category.ParentId);
            return View(category);
        }

        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            var cat = _dbContext.Categories.Include(x=>x.Parent).ToList();
            return Json(new { data = cat });
            //return Json(new { data = _unitOfWork.SP_Call.ReturnList<Category>(SD.usp_GetAllCategory,null)  });
        }



        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            // var objFromDb = _dbContext.Categories.Where(i=>i.CategoryId==id);
            var objFromDb = await _repository.GetById(id);
           
            if (objFromDb == null)
            {
                return Json(new { success = false, message = "Error while deleting." });
            }

            //  _dbContext.Remove(objFromDb);
            //  _dbContext.SaveChanges();
             _dbContext.Categories.Remove(objFromDb);
            _dbContext.SaveChanges();


            return Json(new { success = true, message = "Delete successful." });

        }


        #endregion

    }
}
