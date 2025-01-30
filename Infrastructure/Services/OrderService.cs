using EcomSiteMVC.Core.DTOs;
using EcomSiteMVC.Core.IRepositories;
using EcomSiteMVC.Core.IServices;
using EcomSiteMVC.Core.Models.Entities;

namespace EcomSiteMVC.Infrastructure.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<Order> CreateOrder(PlaceOrderDTO model, string userId, Cart cartProducts)
        {
            var order = new Order
            {
                CustomerId = int.Parse(userId),
                FullName = model.FullName,
                TotalOrderAmount = cartProducts.CartItems.Sum(p => p.UnitPrice * p.Quantity),
                OrderStatus = Core.Enums.Status.Pending,
                PaymentStatus = Core.Enums.Status.Completed,
                PaymentMethod = model.PaymentMethod,
                ShippingAddress = model.Address + "," + model.City + "," + model.ZipCode + "," + model.Province,
            };

            var result = await _orderRepository.Add(order);
            return result;
        }
    }
}
