using AspNetCoreHero.ToastNotification.Abstractions;
using EcomSiteMVC.Core.Enums;
using EcomSiteMVC.Core.IServices;
using EcomSiteMVC.Core.Models.Entities;
using EcomSiteMVC.Extensions.KhaltiPaymentService.Config;
using EcomSiteMVC.Extensions.KhaltiPaymentService.Model;
using EcomSiteMVC.Extensions.KhaltiPaymentService.Service;
using Microsoft.AspNetCore.Mvc;
using System.Web;

namespace EcomSiteMVC.Infrastructure.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IOrderService _orderService;
        private readonly IUserService _userService;
        private readonly INotyfService _notyf;
        private readonly IKhaltiService _khaltiService;
        private readonly KhaltiConfig _khaltiConfig;

        public PaymentService(
            IOrderService orderService, IUserService userService, INotyfService notyf,
            IKhaltiService khaltiService, KhaltiConfig khaltiConfig)
        {
            _orderService = orderService;
            _userService = userService;
            _notyf = notyf;
            _khaltiService = khaltiService;
            _khaltiConfig = khaltiConfig;
        }

        public async Task<IActionResult> ProcessCodPayment(Order order)
        {
            var updatedOrder = await _orderService.UpdateOrderStatus(order.OrderId, Status.Completed.ToString());
            return updatedOrder != null
                ? ShowOrderSuccess("Order placed successfully!", order.OrderId)
                : ShowOrderError("Order failed.");
        }

        public async Task<IActionResult> ProcessKhaltiPayment(Order order)
        {
            var userDetails = await _userService.GetExistingUserProfileAsync(order.CustomerId);
            var request = new KhaltiRequestModel
            {
                ReturnUrl = $"{_khaltiConfig.WebsiteUrl}/Order/VerifyKhaltiPayment?orderId={order.OrderId}",
                WebsiteUrl = _khaltiConfig.WebsiteUrl,
                Amount = order.TotalOrderAmount * 100,
                PurchaseOrderId = order.OrderId.ToString(),
                PurchaseOrderName = "Order Payment",
                CustomerInfo = new CustomerInfo
                {
                    Name = order.FullName,
                    Email = userDetails.Email,
                    Phone = /*userDetails.Phone ??*/ "9800000000"
                },
                ProductDetails = new List<ProductDetail>()
            };

            foreach (var item in order.OrderDetails)
            {
                var productName = await _orderService.GetProductNameByOrderDetail(item);
                request.ProductDetails.Add(new ProductDetail
                {
                    Identity = Guid.NewGuid().ToString(),
                    Name = productName,
                    UnitPrice = item.UnitPrice * 100,
                    TotalPrice = (item.UnitPrice * item.Quantity) * 100,
                    Quantity = item.Quantity,
                });
            }

            var paymentRes = await _khaltiService.InitiatePayment(request);
            return !string.IsNullOrEmpty(paymentRes.PaymentUrl)
                ? new RedirectResult(paymentRes.PaymentUrl)
                : ShowOrderError("Error during payment.");
        }

        public async Task<IActionResult> VerifyKhaltiPayment(int orderId, string pidx)
        {
            var verifyResponse = await _khaltiService.VerifyPayment(pidx);
            if (verifyResponse.Status.Equals(Status.Completed.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                await _orderService.UpdateOrderStatus(orderId, Status.Completed.ToString());
                return ShowOrderSuccess("Payment Successful!", orderId);
            }
            return ShowOrderError("Payment Failed.");
        }

        #region helpers
        public (string orderId, string pidx) ExtractKhaltiPaymentDetails(string queryString)
        {
            var query = HttpUtility.ParseQueryString(queryString.TrimStart('?'));
            var orderId = query["orderId"] ?? "";
            var pidx = query["pidx"] ?? "";
            return (orderId, pidx);
        }

        private IActionResult ShowOrderSuccess(string message, int? orderId = null)
        {
            _notyf.Success(message);
            return orderId.HasValue ? new RedirectToActionResult("OrderSuccess", "Order", new { orderId })
                                    : new RedirectToActionResult("OrderConfirmation", "Order", null);
        }

        private IActionResult ShowOrderError(string message)
        {
            _notyf.Error(message);
            return new RedirectToActionResult("OrderConfirmation", "Order", null);
        }
        #endregion
    }
}
