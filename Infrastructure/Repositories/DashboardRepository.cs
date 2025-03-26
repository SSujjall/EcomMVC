using EcomSiteMVC.Core.DTOs;
using EcomSiteMVC.Core.IRepositories;
using EcomSiteMVC.Infrastructure.Data.Contexts;
using Microsoft.EntityFrameworkCore;

namespace EcomSiteMVC.Infrastructure.Repositories
{
    public class DashboardRepository : IDashboardRepository
    {
        private readonly AppDbContext _dbContext;
        public DashboardRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> GetTotalNoOfSales()
        {
            var totalSales = await _dbContext.Orders
                                .Join(_dbContext.OrderDetails,
                                    order => order.OrderId,
                                    orderDetails => orderDetails.OrderId,
                                    (order, orderDetails) => new { order, orderDetails })
                                .Where(joined => joined.order.OrderStatus != "Cancelled")
                                .SumAsync(joined => joined.orderDetails.Quantity);

            return totalSales;
        }

        public async Task<int> GetTotalNoOfOrders()
        {
            var totalOrders = await _dbContext.Orders.Where(o => o.OrderStatus != "Cancelled").CountAsync();
            return totalOrders;
        }

        public async Task<IEnumerable<MonthlySalesDTO>> GetMonthlySales()
        {
            var monthlySales = await _dbContext.OrderDetails
                                    .GroupBy(od => new { od.Order.OrderDate.Month, od.Order.OrderDate.Year })
                                    .Select(g => new MonthlySalesDTO
                                    {
                                        Month = $"{g.Key.Month}/{g.Key.Year}",
                                        TotalQuantity = g.Sum(x => x.Quantity),
                                        TotalRevenue = g.Sum(x => x.Quantity * x.UnitPrice)
                                    })
                                    .ToListAsync();
            return monthlySales;
        }

        public async Task<IEnumerable<TopCategorySalesDTO>> GetTopCategorySales()
        {
            var categorySales =  await _dbContext.OrderDetails
                                    .GroupBy(od => od.Product.Category)
                                    .Select(g => new TopCategorySalesDTO
                                    {
                                        CategoryName = g.Key.CategoryName,
                                        SalesAmount = g.Sum(x => x.Quantity * x.UnitPrice)
                                    })
                                    .OrderByDescending(tc => tc.SalesAmount)
                                    .Take(5)
                                    .ToListAsync();
            return categorySales;
        }
    }
}
