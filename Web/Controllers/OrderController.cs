using AspNetCoreHero.ToastNotification.Abstractions;
using EcomSiteMVC.Core.DTOs;
using EcomSiteMVC.Core.Enums;
using EcomSiteMVC.Core.IServices;
using EcomSiteMVC.Core.Models.Entities;
using EcomSiteMVC.Infrastructure.Data.Contexts;
using EcomSiteMVC.Utilities.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace EcomSiteMVC.Web.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly ICartService _cartService;
        private readonly IOrderService _orderService;
        private readonly IUserService _userService;
        private readonly INotyfService _notyf;
        private readonly IPaymentService _paymentService;
        private readonly IProductService _productService;
        private readonly AppDbContext _appDbContext;
        public OrderController(ICartService cartService, IUserService userService, INotyfService notyf,
                               IOrderService orderService, IPaymentService paymentService, IProductService productService,
                               AppDbContext dbContext)
        {
            _cartService = cartService;
            _userService = userService;
            _notyf = notyf;
            _orderService = orderService;
            _paymentService = paymentService;
            _productService = productService;
            _appDbContext = dbContext;
        }

        [Authorize]
        public async Task<IActionResult> UserOrdersView()
        {
            var userId = int.Parse(User.FindFirst("UserId").Value);
            if (userId == 0)
            {
                _notyf.Error("User not found.");
                return RedirectToAction("CustomerProductView", "Product");
            }

            var orders = await _orderService.GetUserOrderHistory(userId);
            if (!orders.Any() || orders == null)
            {
                return View(new List<Order>());
            }
            return View(orders);
        }

        [HttpGet]
        public async Task<IActionResult> GetOrderDetails(string orderId)
        {
            try
            {
                if (string.IsNullOrEmpty(orderId))
                {
                    return BadRequest("Order ID is required");
                }

                var decryptedId = orderId.DecryptParameter();
                if (string.IsNullOrEmpty(decryptedId))
                {
                    return BadRequest("Invalid Order ID");
                }

                var orderIdInt = int.Parse(decryptedId);
                var result = await _orderService.GetOrderDetailsByOrderId(orderIdInt);
                return Json(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while processing your request: " + ex);
            }
        }


        [Authorize]
        public async Task<IActionResult> BuyNow(int productId, int quantity)
        {
            var userId = int.Parse(User.FindFirst("UserId")?.Value ?? "0");
            if (userId == 0)
            {
                _notyf.Error("User not logged in.");
                return RedirectToAction("LoginView", "Auth");
            }

            // Create a temporary cart-like structure for the single product
            var product = await _productService.GetProductById(productId);
            if (product == null)
            {
                _notyf.Error("Product not found.");
                return RedirectToAction("CustomerProductView", "Product");
            }

            #region temp cart model
            var singleItemCart = new Cart
            {
                CustomerId = userId,
                CartItems = new List<CartItem>
                {
                    new CartItem
                    {
                        ProductId = product.ProductId,
                        Quantity = quantity,
                        UnitPrice = product.Price,
                        Product = product
                    }
                }
            };
            #endregion

            var settings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };

            // Store the temporary cart in Session
            var serializedCart = JsonConvert.SerializeObject(singleItemCart, settings);
            HttpContext.Session.SetString("BuyNowCart", serializedCart);

            ViewBag.UserDetail = await _userService.GetExistingUserProfileAsync(userId);
            ViewBag.IsBuyNow = true;
            return View("OrderConfirmation", singleItemCart); // instead of creating separate confirmation page for "BuyNow" function, we redirect to already existing OrderConfirmation View Page.
        }

        public async Task<IActionResult> OrderConfirmation()
        {
            var userId = int.Parse(User.FindFirst("UserId").Value);
            var cart = await _cartService.GetCartByUserIdAsync(userId);
            if (cart == null || !cart.CartItems.Any())
            {
                _notyf.Error("Your cart is empty.");
                return RedirectToAction("CartView", "Cart");
            }

            ViewBag.UserDetail = await _userService.GetExistingUserProfileAsync(userId);
            return View(cart);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> PlaceOrder(PlaceOrderDTO model)
        {
            var userId = int.Parse(User.FindFirst("UserId")?.Value ?? "0");
            if (userId == 0)
            {
                _notyf.Error("User not logged in.");
                return RedirectToAction("LoginView", "Auth");
            }

            Cart cart;
            var buyNowCartJson = HttpContext.Session.GetString("BuyNowCart");
            if (!string.IsNullOrEmpty(buyNowCartJson))
            {
                cart = JsonConvert.DeserializeObject<Cart>(buyNowCartJson);
                HttpContext.Session.Remove("BuyNowCart"); // Clear after use
            }
            else
            {
                cart = await _cartService.GetCartByUserIdAsync(userId);
                if (cart == null || !cart.CartItems.Any())
                {
                    _notyf.Error("Your cart is empty.");
                    return RedirectToAction("CartView", "Cart");
                }
            }

            var order = await _orderService.CreateOrder(model, userId.ToString(), cart);

            switch (model.PaymentMethod)
            {
                case "khalti":
                    return await _paymentService.ProcessKhaltiPayment(order);
                case "cod":
                    return await _paymentService.ProcessCodPayment(order);
                default:
                    _notyf.Error("Payment method invalid!");
                    return RedirectToAction("OrderConfirmation");
            }
        }

        public async Task<IActionResult> VerifyKhaltiPayment()
        {
            var (orderId, pidx) = _paymentService.ExtractKhaltiPaymentDetails(Request.QueryString.Value);
            if (string.IsNullOrEmpty(orderId) || string.IsNullOrEmpty(pidx))
            {
                _notyf.Error("Invalid payment details.");
                return RedirectToAction("OrderConfirmation");
            }

            return await _paymentService.VerifyKhaltiPayment(int.Parse(orderId), pidx);
        }

        public async Task<IActionResult> OrderSuccess(int orderId)
        {
            var userId = int.Parse(User.FindFirst("UserId")?.Value ?? "0");
            // Clear cart after order is created
            var order = await _orderService.GetOrderById(orderId);

            if (order.PaymentStatus.Equals(Status.Completed.ToString(), StringComparison.CurrentCultureIgnoreCase))
            {
                await _cartService.ClearCart(userId);
            }

            return View(order);
        }
    }
}
