using EcomSiteMVC.Core.IRepositories;
using EcomSiteMVC.Core.Models.Entities;
using EcomSiteMVC.Infrastructure.Data.Contexts;
using Microsoft.EntityFrameworkCore;

namespace EcomSiteMVC.Infrastructure.Repositories
{
    public class OrderRepository : RepositoryBase<Order>, IOrderRepository
    {
        private readonly AppDbContext _dbContext;
        public OrderRepository(AppDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Order> GetOrderWithDetailsByOrderId(int orderId)
        {
            return await _dbContext.Orders
            .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Product)
            .FirstOrDefaultAsync(o => o.OrderId == orderId);
        }
    }
}
