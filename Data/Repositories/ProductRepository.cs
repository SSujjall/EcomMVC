using EcomSiteMVC.Interfaces.IRepositories;
using EcomSiteMVC.Models.Entities;

namespace EcomSiteMVC.Data.Repositories
{
    public class ProductRepository : RepositoryBase<Product>, IProductRepository
    {
        public ProductRepository(AppDbContext dbContext) : base(dbContext)
        {
        }
    }
}
