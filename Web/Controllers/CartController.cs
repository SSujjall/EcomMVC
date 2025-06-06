﻿using AspNetCoreHero.ToastNotification.Abstractions;
using EcomSiteMVC.Core.DTOs;
using EcomSiteMVC.Core.IServices;
using EcomSiteMVC.Core.Models.Entities;
using Microsoft.AspNetCore.Authorization;
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

            if (cart?.CartItems == null)
            {
                return View(new Cart());
            }
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
            return Redirect(Request.Headers["Referer".ToString() ?? "/"]);
        }

        [Authorize]
        public async Task<IActionResult> DeleteItem(int id)
        {
            var result = await _cartService.DeleteCartItem(id);

            if (result)
            {
                _notyf.Success("Product deleted successfully!", 5);
                return Redirect(Request.Headers["Referer".ToString() ?? "/"]);
            }

            _notyf.Error("Failed to delete product!", 5);
            return Redirect(Request.Headers["Referer".ToString() ?? "/"]);
        }

        public async Task<IActionResult> AddQuantity(int id)
        {
            var result = await _cartService.AddQuantity(id);
            if (result)
            {
                _notyf.Success("Quantity Added successfully!", 5);
                return Redirect(Request.Headers["Referer".ToString() ?? "/"]);
            }

            _notyf.Error("Failed to add quantity!", 5);
            return Redirect(Request.Headers["Referer".ToString() ?? "/"]);
        }

        public async Task<IActionResult> SubstractQuantity(int id)
        {
            var result = await _cartService.SubstractQuantity(id);
            if (result)
            {
                _notyf.Success("Quantity Reduced successfully!", 5);
                return Redirect(Request.Headers["Referer".ToString() ?? "/"]);
            }

            _notyf.Error("Failed to remove quantity!", 5);
            return Redirect(Request.Headers["Referer".ToString() ?? "/"]);
        }

        private int GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst("UserId");
            return userIdClaim != null ? int.Parse(userIdClaim.Value) : 0;
        }
    }
}
