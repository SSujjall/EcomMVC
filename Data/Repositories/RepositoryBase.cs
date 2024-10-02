using EcomSiteMVC.Interfaces.IRepositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace EcomSiteMVC.Data.Repositories
{
    public class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        private readonly AppDbContext _dbContext;

        public RepositoryBase(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<T> Add(T entity)
        {
            var result = await _dbContext.AddAsync(entity);
            _dbContext.SaveChanges();
            return result.Entity;
        }

        public async Task Delete(T entity)
        {
            if (entity != null)
            {
                _dbContext.Remove(entity);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<T>? GetById(object id)
        {
            var result = await _dbContext.FindAsync<T>(id);
            return result;
        }

        public async Task<T> Update(T entity)
        {
            var result = _dbContext.Update(entity);
            await _dbContext.SaveChangesAsync();
            return result.Entity;
        }

        public async Task<T> FindByConditionAsync(Expression<Func<T, bool>> expression)
        {
            return await _dbContext.Set<T>().FirstOrDefaultAsync(expression);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            var result = await _dbContext.Set<T>().ToListAsync();
            return result;
        }
    }
}