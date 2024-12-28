using AspNetCoreHero.ToastNotification.Abstractions;
using EcomSiteMVC.Core.DTOs;
using EcomSiteMVC.Core.IServices;
using Microsoft.AspNetCore.Mvc;

namespace EcomSiteMVC.Web.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService _cartService;
        private readonly INotyfService _notyf;

        public CartController(ICartService cartService, INotyfService notyf)
        {
            _cartService = cartService;
            _notyf = notyf;
        }

        public async Task<IActionResult> CartView()
        {
            var cart = await _cartService.GetCartByUserIdAsync(GetCurrentUserId());
            return View(cart);
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(AddToCartDTO model)
        {
            var currentUserId = GetCurrentUserId(); // Ensure this method gets the correct UserId
            model.CustomerId = currentUserId;

            if (currentUserId == 0)
            {
                _notyf.Error("Login to add to cart.");
                return RedirectToAction("LoginView", "Auth");
            }

            var success = await _cartService.AddToCart(model);
            if (success)
            {
                _notyf.Success("Product added to cart!", 5);
            }
            else
            {
                _notyf.Error("Failed to add product to cart.");
            }
            return RedirectToAction("CartView");
        }

        private int GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst("UserId");
            return userIdClaim != null ? int.Parse(userIdClaim.Value) : 0;
        }
    }
}
