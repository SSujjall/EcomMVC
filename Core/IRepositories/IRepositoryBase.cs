using System.Linq.Expressions;

namespace EcomSiteMVC.Core.IRepositories
{
    public interface IRepositoryBase<T> where T : class
    {
        Task<T> Add(T entity);
        Task<T> Update(T entity);
        Task Delete(T entity);
        Task<T>? GetById(object id);
        Task<T> FindSingleByConditionAsync(Expression<Func<T, bool>> expression);
        public Task<IEnumerable<T>> FindAllByConditionAsync(Expression<Func<T, bool>> expression);
        public Task<IEnumerable<T>> GetAllAsync();
    }
}
