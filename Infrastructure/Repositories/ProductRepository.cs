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

        public async Task<IEnumerable<Product>> FilterPopularProducts(int topN)
        {
            var popularProducts = await _dbContext.OrderDetails
                                                  .GroupBy(od => od.ProductId)
                                                  .Select(x => new
                                                  {
                                                      ProductId = x.Key,
                                                      OrderCount = x.Count(),
                                                      TotalSales = x.Sum(od => od.Quantity * od.UnitPrice)
                                                  })
                                                  .OrderByDescending(x => x.OrderCount)
                                                  .ThenByDescending(x => x.TotalSales)
                                                  .Take(topN)
                                                  .Join(_dbContext.Products,
                                                         od => od.ProductId,
                                                         p => p.ProductId,
                                                         (od, p) => p)
                                                  .ToListAsync();
            return popularProducts;
        }
    }
}
