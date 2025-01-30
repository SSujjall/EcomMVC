using EcomSiteMVC.Core.DTOs;
using EcomSiteMVC.Core.Models.Entities;
using EcomSiteMVC.Extensions.KhaltiPaymentService.Model;

namespace EcomSiteMVC.Core.IServices
{
    public interface IOrderService
    {
        Task<Order> CreateOrder(PlaceOrderDTO model, string userId, Cart cartProducts);
        Task<Order> UpdateOrderStatus(int orderId, string status);
        //Task<PaymentInitiateResponse> KhaltiPaymentInitiate(int orderId);
    }
}
