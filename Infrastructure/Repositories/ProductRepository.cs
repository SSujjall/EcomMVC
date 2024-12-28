using EcomSiteMVC.Core.IRepositories;
using EcomSiteMVC.Core.Models.Entities;
using EcomSiteMVC.Infrastructure.Data.Contexts;
using Microsoft.EntityFrameworkCore;

namespace EcomSiteMVC.Infrastructure.Repositories
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
