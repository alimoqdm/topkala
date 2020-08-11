using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        private readonly TopKala3Contex _db;
        public CategoryRepository(TopKala3Contex dbContext)
      : base(dbContext)
        {
            _db = dbContext;
        }

        public bool CategoryExists(int id)
        {
            return _db.Categories.Any(e => e.CategoryId == id);
        }

        public async Task<Category> GetDetalis(int? id)
        {
            return await _db.Categories
                .Include(c => c.Parent)
                .FirstOrDefaultAsync(m => m.CategoryId == id);
        }
    }
}
