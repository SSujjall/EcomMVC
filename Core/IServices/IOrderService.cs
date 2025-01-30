using EcomSiteMVC.Core.DTOs;
using EcomSiteMVC.Core.Models.Entities;

namespace EcomSiteMVC.Core.IServices
{
    public interface IOrderService
    {
        Task<Order> CreateOrder(PlaceOrderDTO model, string userId, Cart cartProducts);

        Task<>
    }
}
