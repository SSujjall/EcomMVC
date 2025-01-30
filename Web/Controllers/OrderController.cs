using AspNetCoreHero.ToastNotification.Abstractions;
using EcomSiteMVC.Core.DTOs;
using EcomSiteMVC.Core.Enums;
using EcomSiteMVC.Core.IRepositories;
using EcomSiteMVC.Core.IServices;
using EcomSiteMVC.Core.Models.Entities;
using EcomSiteMVC.Extensions.KhaltiPaymentService.Config;
using EcomSiteMVC.Extensions.KhaltiPaymentService.Model;
using EcomSiteMVC.Extensions.KhaltiPaymentService.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EcomSiteMVC.Web.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly ICartService _cartService;
        private readonly IOrderService _orderService;
        private readonly IUserService _userService;
        private readonly INotyfService _notyf;
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;
        private readonly KhaltiConfig _khaltiConfig;
        private readonly IKhaltiService _khaltiService;

        public OrderController(ICartService cartService, IUserService userService, 
                               INotyfService notyf, IOrderService orderService,
                               IOrderRepository orderRepository, KhaltiConfig khaltiConfig,
                               IKhaltiService khaltiService, IProductRepository productRepository)
        {
            _cartService = cartService;
            _userService = userService;
            _notyf = notyf;
            _orderService = orderService;
            _orderRepository = orderRepository;
            _khaltiConfig = khaltiConfig;
            _khaltiService = khaltiService;
            _productRepository = productRepository;
        }

        public async Task<IActionResult> OrderConfirmation()
        {
            var userId = int.Parse(User.FindFirst("UserId").Value);
            var cartProducts = await _cartService.GetCartByUserIdAsync(userId);
            var userDetails = await _userService.GetExistingUserProfileAsync(userId);

            if (cartProducts == null || !cartProducts.CartItems.Any())
            {
                _notyf.Error("Your cart is empty.");
                return RedirectToAction("CartView", "Cart");
            }

            ViewBag.UserDetail = userDetails;
            return View(cartProducts);
        }

        [HttpPost]
        public async Task<IActionResult> PlaceOrder(PlaceOrderDTO model, Cart cart)
        {
            var userId = User.FindFirst("UserId").Value;
            var cartProducts = await _cartService.GetCartByUserIdAsync(int.Parse(userId));

            if (cartProducts == null || !cartProducts.CartItems.Any())
            {
                _notyf.Error("Your cart is empty.");
                return RedirectToAction("CartView", "Cart");
            }

            var order = await _orderService.CreateOrder(model, userId, cartProducts);
            var orderId = order.OrderId;

            switch (model.PaymentMethod)
            {
                case "khalti":
                    return await KhaltiPayment(orderId);
                case "cod":
                    return await CodPayment(orderId);
                default:
                    return RedirectToAction("OrderConfirmation");
            }
        }

        public async Task<IActionResult> KhaltiPayment(int orderId)
        {
            var order = await _orderRepository.GetById(orderId);
            if (order == null)
            {
                _notyf.Error("Order not found.");
                return RedirectToAction("OrderConfirmation");
            }

            var userDetails = await _userService.GetExistingUserProfileAsync(order.CustomerId);

            var request = new KhaltiRequestModel
            {
                ReturnUrl = Url.Action("VerifyKhaltiPayment", "Order", new { orderId }, Request.Scheme),
                WebsiteUrl = _khaltiConfig.WebsiteUrl,
                Amount = order.TotalOrderAmount * 100,
                PurchaseOrderId = orderId.ToString(),
                PurchaseOrderName = "Order Payment",
                CustomerInfo = new CustomerInfo
                {
                    Name = order.FullName,
                    Email = userDetails.Email,
                    Phone = "9800000000" // placeholder used for now, TODO: CHANGE this to actual value later (add to User model)
                },
                ProductDetails = new List<ProductDetail>()
            };

            foreach (var item in order.OrderDetails)
            {
                var product = await _productRepository.FindByConditionAsync(p => p.ProductId == item.ProductId);
                request.ProductDetails.Add(new ProductDetail
                {
                    Identity = Guid.NewGuid().ToString(),
                    Name = product.ProductName,
                    UnitPrice = item.UnitPrice * 100,
                    TotalPrice = (item.UnitPrice * item.Quantity) * 100,
                    Quantity = item.Quantity,
                });
            }

            var paymentRes = await _khaltiService.InitiatePayment(request);

            if (!string.IsNullOrEmpty(paymentRes.PaymentUrl))
            {
                return Redirect(paymentRes.PaymentUrl);
            }

            _notyf.Error("Error occured during payment");
            return RedirectToAction("OrderConfirmation");
        }


        public async Task<IActionResult> VerifyKhaltiPayment()
        {
            var url = Request.QueryString.Value; // Gets the full query string
            string orderId = "";
            string pidx = "";

            // First, split by "orderId=" to get everything after it
            var orderIdPart = url.Split("orderId=")[1];

            // Then split by "&" to get just the orderId?pidx part
            orderIdPart = orderIdPart.Split('&')[0];

            // Finally split by "?" to separate orderId and pidx
            if (orderIdPart.Contains("?"))
            {
                var parts = orderIdPart.Split('?');
                orderId = parts[0];  // Will be whatever orderid is
                pidx = parts[1].Split('=')[1];  // Will be whatever pidx is in the url
            }

            var verifyResponse = await _khaltiService.VerifyPayment(pidx);
            if (verifyResponse.Status.ToLower() == Status.Completed.ToString().ToLower())
            {
                await _orderService.UpdateOrderStatus(int.Parse(orderId), Status.Completed.ToString());
                _notyf.Success("Payment Successful");
                return RedirectToAction("OrderSuccess", new { orderId });
            }
            _notyf.Error("Payment Failed");
            return RedirectToAction("OrderConfirmation");
        }

        public async Task<IActionResult> CodPayment(int orderId)
        {
            await _orderService.UpdateOrderStatus(orderId, Status.Completed.ToString());
            _notyf.Success("Order placed successfully");
            return RedirectToAction("OrderSuccess", new { orderId });
        }

        public async Task<IActionResult> OrderSuccess(int orderId)
        {
            var order = await _orderRepository.GetById(orderId);
            return View(order);
        }
    }
}
