using AspNetCoreHero.ToastNotification.Abstractions;
using EcomSiteMVC.Core.DTOs;
using EcomSiteMVC.Core.IServices;
using EcomSiteMVC.Core.Models.Entities;
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

        public OrderController(ICartService cartService, IUserService userService, INotyfService notyf, IOrderService orderService)
        {
            _cartService = cartService;
            _userService = userService;
            _notyf = notyf;
            _orderService = orderService;
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
        public async Task<IActionResult> PlaceOrder(PlaceOrderDTO model)
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
                    await KhaltiPayment(orderId);
                    return View();
                case "cod":
                    await CodPayment(orderId);
                    return View();
                default:
                    return View();
            }
        }

        public async Task<IActionResult> KhaltiPayment(int orderId)
        {
            return null;
        }

        public async Task<IActionResult> CodPayment(int orderId)
        {
            return null;
        }
    }
}
