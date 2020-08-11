using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public interface ICategoryRepository: IGenericRepository<Category>
    {

       Task<Category> GetDetalis(int? id);
        bool CategoryExists(int id);
    }
}
