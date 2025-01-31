using EcomSiteMVC.Core.DTOs;
using EcomSiteMVC.Core.IRepositories;
using EcomSiteMVC.Core.IServices;
using EcomSiteMVC.Core.Models.Entities;
using EcomSiteMVC.Extensions.KhaltiPaymentService.Config;
using EcomSiteMVC.Extensions.KhaltiPaymentService.Model;
using EcomSiteMVC.Extensions.KhaltiPaymentService.Service;
using EcomSiteMVC.Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace EcomSiteMVC.Infrastructure.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IRepositoryBase<OrderDetail> _orderDetailRepository;
        private readonly IProductRepository _productRepository;
        private readonly IKhaltiService _khaltiService;
        private readonly KhaltiConfig _khaltiConfig;

        public OrderService(IOrderRepository orderRepository, IKhaltiService khaltiService,
                            KhaltiConfig khaltiConfig, IRepositoryBase<OrderDetail> orderDetailRepository,
                            IProductRepository productRepository)
        {
            _orderRepository = orderRepository;
            _khaltiService = khaltiService;
            _khaltiConfig = khaltiConfig;
            _orderDetailRepository = orderDetailRepository;
            _productRepository = productRepository;
        }

        public async Task<Order> CreateOrder(PlaceOrderDTO model, string userId, Cart cartProducts)
        {
            var order = new Order
            {
                CustomerId = int.Parse(userId),
                FullName = model.FullName,
                TotalOrderAmount = cartProducts.CartItems.Sum(p => p.UnitPrice * p.Quantity),
                OrderStatus = Core.Enums.Status.Pending.ToString(),
                PaymentStatus = Core.Enums.Status.Pending.ToString(),
                PaymentMethod = model.PaymentMethod,
                ShippingAddress = model.Address + "," + model.City + "," + model.ZipCode + "," + model.Province
            };

            var orderResult = await _orderRepository.Add(order);

            foreach (var item in cartProducts.CartItems)
            {
                var orderDetailModel = new OrderDetail
                {
                    OrderId = orderResult.OrderId,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice,
                    TotalAmount = (item.UnitPrice * item.Quantity).ToString(),
                };
                await _orderDetailRepository.Add(orderDetailModel);
            }

            return orderResult;
        }

        public async Task<Order> UpdateOrderStatus(int orderId, string status)
        {
            var existingOrder = await _orderRepository.GetById(orderId);
            if (existingOrder != null)
            {
                existingOrder.PaymentStatus = status;
                var result = await _orderRepository.Update(existingOrder);
                return result;
            }
            return null;
        }

        public Task<Order> GetOrderById(int id)
        {
            var order = _orderRepository.GetById(id);
            if (order == null)
            {
                return null;
            }
            return order;
        }

        public async Task<string> GetProductNameByOrderDetail(OrderDetail model)
        {
            var product = await _productRepository.FindByConditionAsync(p => p.ProductId == model.ProductId);
            if (product == null)
            {
                return string.Empty;
            }
            return product.ProductName.ToString();
        }
    }
}
