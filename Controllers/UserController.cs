using AspNetCoreHero.ToastNotification.Abstractions;
using EcomSiteMVC.Interfaces.IServices;
using EcomSiteMVC.Migrations;
using EcomSiteMVC.Models.DTOs;
using EcomSiteMVC.Models.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EcomSiteMVC.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly ICloudinaryService _cloudinaryService;
        private readonly string profilePictureFolderName = FolderName.ProfilePictures.ToString();
        private readonly INotyfService _notyf;

        public UserController(IUserService userService, ICloudinaryService cloudinaryService, INotyfService notyf)
        {
            _userService = userService;
            _cloudinaryService = cloudinaryService;
            _notyf = notyf;
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

            // Get the existing user profile
            var existingProfile = await _userService.GetUserProfileAsync(userId);

            if (profileImage != null && profileImage.Length > 0)
            {
                // If there's a new image, upload it
                var imageUrl = await _cloudinaryService.UploadImageAsync(profileImage, FolderName.ProfilePictures);

                if (imageUrl != null)
                {
                    // If there's an existing image, delete it
                    if (!string.IsNullOrEmpty(existingProfile?.ProfileImage))
                    {
                        // Extract public ID from the existing image URL
                        var existingPublicId = existingProfile.ProfileImage.Split('/').Last().Split('.').First();
                        await _cloudinaryService.DeleteImageAsync($"{profilePictureFolderName}/{existingPublicId}");
                    }
                    // Update the model with the new image URL
                    model.ProfileImage = imageUrl;
                }
            }
            else
            {
                // If there is no new image, retain the existing image
                model.ProfileImage = existingProfile?.ProfileImage;
            }

            var profileUpdate = await _userService.CreateUserProfileAsync(model, userId);

            if (profileUpdate)
            {
                _notyf.Success("Profile updated successfully!", 5);
            }
            else
            {
                _notyf.Error("Profile update failed.", 5);
            }
            return RedirectToAction("ProfileView");
        }

    }
}
