using AspNetCoreHero.ToastNotification.Abstractions;
using EcomSiteMVC.Core.DTOs;
using EcomSiteMVC.Core.Enums;
using EcomSiteMVC.Core.IServices;
using EcomSiteMVC.Core.Models.ViewModels;
using EcomSiteMVC.Extensions.EmailService.Model;
using EcomSiteMVC.Extensions.EmailService.Service;
using EcomSiteMVC.Utilities;
using EcomSiteMVC.Utilities.ExternalServices.CloudinaryService.Service;
using EcomSiteMVC.Utilities.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EcomSiteMVC.Web.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly ICloudinaryService _cloudinaryService;
        private readonly string profilePictureFolderName = FolderName.ProfilePictures.ToString();
        private readonly INotyfService _notyf;
        private readonly IEmailService _emailService;

        public UserController(IUserService userService, ICloudinaryService cloudinaryService, INotyfService notyf, IEmailService emailService)
        {
            _userService = userService;
            _cloudinaryService = cloudinaryService;
            _notyf = notyf;
            _emailService = emailService;
        }

        public async Task<IActionResult> ProfileView()
        {
            var userId = int.Parse(User.FindFirst("UserId")?.Value);
            var profileData = await _userService.GetExistingUserProfileAsync(userId);
            return View(profileData);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProfile(UserDTO model, IFormFile profileImage)
        {
            var userId = int.Parse(User.FindFirst("UserId")?.Value);

            // Get the existing user profile
            var existingProfile = await _userService.GetExistingUserProfileAsync(userId);

            if (profileImage == null)
            {
                if (existingProfile.UserProfile == null)
                {
                    _notyf.Error("Profile image is required. Please upload an image.", 5);
                    return Redirect(Request.Headers["Referer".ToString() ?? "/"]);
                }
                else
                {
                    // Retain the existing image if it exists
                    model.UserProfile.ProfileImage = existingProfile.UserProfile.ProfileImage;
                }

            }
            else
            {
                // If there's a new image, upload it
                var imageUrl = await _cloudinaryService.UploadImageAsync(profileImage, FolderName.ProfilePictures);

                if (imageUrl != null)
                {
                    // If there's an existing image, delete it
                    if (!string.IsNullOrEmpty(existingProfile?.UserProfile?.ProfileImage))
                    {
                        // Extract public ID from the existing image URL
                        var existingPublicId = existingProfile.UserProfile.ProfileImage.Split('/').Last().Split('.').First();
                        await _cloudinaryService.DeleteImageAsync($"{profilePictureFolderName}/{existingPublicId}");
                    }
                    // Update the model with the new image URL
                    model.UserProfile.ProfileImage = imageUrl;
                }
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

        #region User Settings Section
        public IActionResult UserSettingsView()
        {
            var viewModel = new UserSettingsViewModel
            {
                ChangePasswordDTO = new ChangePasswordDTO(),
                UserPasswordUpdateDTO = new UserPasswordUpdateDTO()
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> ChangeUserPassword(ChangePasswordDTO model)
        {
            var userId = int.Parse(User.FindFirst("UserId")?.Value);

            var otp = await _userService.GenerateOtpForPasswordChange(userId);
            if (otp != null)
            {
                var user = await _userService.GetUserById(userId);
                var emailMessage = new EmailMessage(new[] { user.Email }, "Change Password OTP", $"Your OTP: {otp}");
                //_emailService.SendEmail(emailMessage);

                _notyf.Success("Password Change OTP Sent To Email", 5);
                TempData["VerifyOtp"] = true;

                #region
                TempData["OldPassword"] = model.OldPassword;
                TempData["NewPassword"] = model.NewPassword;
                #endregion

                return RedirectToAction("UserSettingsView");
            }
            _notyf.Error("User not found.", 5);
            return Redirect(Request.Headers["Referer".ToString() ?? "/"]);
        }

        public IActionResult VerifyOtpView()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> VerifyOtp(string otp)
        {
            var userId = int.Parse(User.FindFirst("UserId")?.Value);
            var isValidOtp = await _userService.VerifyOtpForPasswordChange(userId, otp);
            if (!isValidOtp)
            {
                _notyf.Error("Invalid OTP.", 5);
                return RedirectToAction("UserSettingsView");
            }

            var oldPassword = TempData["OldPassword"]?.ToString();
            var newPassword = TempData["NewPassword"]?.ToString();

            var passwordChanged = await _userService.ChangeUserPassword(userId, oldPassword, newPassword);
            if (!passwordChanged)
            {
                _notyf.Error("Incorrect Old Password or Error Changing Password.", 5);
                return RedirectToAction("UserSettingsView");
            }

            _notyf.Success("Password Changed Successfully.", 5);
            TempData.Remove("VerifyOtp");
            return RedirectToAction("UserSettingsView");
        }
        #endregion
    }
}
