using EcomSiteMVC.Core.Models.Entities;

namespace EcomSiteMVC.Core.IRepositories
{
    public interface IOrderRepository : IRepositoryBase<Order>
    {
        Task<Order> GetOrderWithDetailsByOrderId(int orderId);
    }
}
