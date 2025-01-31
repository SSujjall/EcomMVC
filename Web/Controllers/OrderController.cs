using AspNetCoreHero.ToastNotification.Abstractions;
using EcomSiteMVC.Core.DTOs;
using EcomSiteMVC.Core.IServices;
using EcomSiteMVC.Core.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

        public OrderController(ICartService cartService, IUserService userService, INotyfService notyf,
                               IOrderService orderService, IPaymentService paymentService, IProductService productService)
        {
            _cartService = cartService;
            _userService = userService;
            _notyf = notyf;
            _orderService = orderService;
            _paymentService = paymentService;
            _productService = productService;
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
            return View("OrderConfirmation", singleItemCart);
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
            var order = await _orderService.GetOrderById(orderId);
            return View(order);
        }
    }
}
