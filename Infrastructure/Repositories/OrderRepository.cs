using EcomSiteMVC.Core.IRepositories;
using EcomSiteMVC.Core.Models.Entities;
using EcomSiteMVC.Infrastructure.Data.Contexts;

namespace EcomSiteMVC.Infrastructure.Repositories
{
    public class OrderRepository : RepositoryBase<Order>, IOrderRepository
    {
        public OrderRepository(AppDbContext dbContext) : base(dbContext)
        {
        }
    }
}
