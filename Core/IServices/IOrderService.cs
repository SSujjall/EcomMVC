using EcomSiteMVC.Core.DTOs;
using EcomSiteMVC.Core.Models.Entities;
using EcomSiteMVC.Extensions.KhaltiPaymentService.Model;

namespace EcomSiteMVC.Core.IServices
{
    public interface IOrderService
    {
        Task<Order> GetOrderById(int id);
        Task<Order> CreateOrder(PlaceOrderDTO model, string userId, Cart cartProducts);
        Task<Order> UpdateOrderStatus(int orderId, string status);
        Task<string> GetProductNameByOrderDetail(OrderDetail model);
        Task<IEnumerable<Order>> GetUserOrderHistory(int userId);
        Task<OrderDetailsResponseDTO> GetOrderDetailsByOrderId(int orderId);
    }
}
