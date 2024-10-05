using EcomSiteMVC.Interfaces.IRepositories;
using EcomSiteMVC.Models.Entities;

namespace EcomSiteMVC.Data.Repositories
{
    public class CategoryRepository : RepositoryBase<Category>, ICategoryRepository
    {
        public CategoryRepository(AppDbContext dbContext) : base(dbContext)
        {
        }
    }
}
