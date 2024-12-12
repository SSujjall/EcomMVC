using EcomSiteMVC.Interfaces.IRepositories;
using EcomSiteMVC.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace EcomSiteMVC.Data.Repositories
{
    public class ProductRepository : RepositoryBase<Product>, IProductRepository
    {
        private readonly AppDbContext _dbContext;

        public ProductRepository(AppDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddImage(ProductImage image)
        {
            await _dbContext.ProductImages.AddAsync(image);
            await _dbContext.SaveChangesAsync();
        }
    }
}
