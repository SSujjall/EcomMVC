using EcomSiteMVC.Core.Models.Entities;

namespace EcomSiteMVC.Core.IServices
{
    public interface IOrderService
    {
        Task<Order> PlaceOrder();
    }
}
