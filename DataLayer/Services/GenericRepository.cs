using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity>
    where TEntity : class
    {

        private readonly TopKala3Contex _dbContext;

        public GenericRepository(TopKala3Contex dbContext)
        {
            _dbContext = dbContext;
        }

        public virtual async Task<IEnumerable<TEntity>> Get(Expression<Func<TEntity,object>> include=null)
        
        {
            IQueryable<TEntity> query = _dbContext.Set<TEntity>();

            if(include != null)
            {
                query = query.Include(include);
            }
       
            

            return query.ToList();

        }
        public virtual async Task Create(TEntity entity)
        {
            await _dbContext.Set<TEntity>().AddAsync(entity);
            await _dbContext.SaveChangesAsync();
        }

        public virtual async Task Delete(TEntity entity)
        {
            _dbContext.Set<TEntity>().Remove(entity);
            await _dbContext.SaveChangesAsync();
        }

        public virtual async Task Delete(object id)
        {
            var entity = await GetById(id);
            _dbContext.Set<TEntity>().Remove(entity);
            await _dbContext.SaveChangesAsync();
        }

  

        public virtual async Task<TEntity> GetById(object id)
        {
            return await _dbContext.Set<TEntity>().FindAsync(id);
                   
              
        }

        public virtual async Task Update(TEntity entity)
        {
            _dbContext.Set<TEntity>().Update(entity);
            await _dbContext.SaveChangesAsync();
        }

     
    }
}
