using EcomSiteMVC.Interfaces.IServices;
using EcomSiteMVC.Migrations;
using EcomSiteMVC.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EcomSiteMVC.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly ICloudinaryService _cloudinaryService;

        public UserController(IUserService userService, ICloudinaryService cloudinaryService)
        {
            _userService = userService;
            _cloudinaryService = cloudinaryService;
        }

        public async Task<IActionResult> ProfileView()
        {
            var userId = int.Parse(User.FindFirst("UserId")?.Value);
            var profileDto = await _userService.GetUserProfileAsync(userId);
            return View(profileDto);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProfile(UserProfileUpdateDTO model, IFormFile profileImage)
        {
            var userId = int.Parse(User.FindFirst("UserId")?.Value);

            // Retrieve the existing user profile to check for the current image
            var existingProfile = await _userService.GetUserProfileAsync(userId);

            if (profileImage != null && profileImage.Length > 0)
            {
                var imageUrl = await _cloudinaryService.UploadProfilePictureAsync(profileImage);
                if (imageUrl != null)
                {
                    model.ProfileImage = imageUrl;
                }
            }
            else
            {
                // If no new image is uploaded, keep the existing image
                model.ProfileImage = existingProfile.ProfileImage;
            }


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
