﻿using EcomSiteMVC.Interfaces.IServices;
using EcomSiteMVC.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EcomSiteMVC.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<IActionResult> ProfileView()
        {
            var userId = int.Parse(User.FindFirst("UserId")?.Value);
            var profileDto = await _userService.GetUserProfileAsync(userId);
            return View(profileDto);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProfile(UserProfileUpdateDTO model)
        {
            var userId = int.Parse(User.FindFirst("UserId")?.Value);
            var profileUpdate = await _userService.CreateUserProfileAsync(model, userId);

            if (profileUpdate)
            {
                TempData["ToastMessage"] = "Profile updated successfully!";
                TempData["ToastType"] = "success";
            }
            else
            {
                TempData["ToastMessage"] = "Profile update failed.";
                TempData["ToastType"] = "error";
            }
            return RedirectToAction("ProfileView");
        }
    }
}
