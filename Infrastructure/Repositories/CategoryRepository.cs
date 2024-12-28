using EcomSiteMVC.Core.IRepositories;
using EcomSiteMVC.Core.Models.Entities;
using EcomSiteMVC.Infrastructure.Data.Contexts;

namespace EcomSiteMVC.Infrastructure.Repositories
{
    public class CategoryRepository : RepositoryBase<Category>, ICategoryRepository
    {
        public CategoryRepository(AppDbContext dbContext) : base(dbContext)
        {
        }
    }
}
