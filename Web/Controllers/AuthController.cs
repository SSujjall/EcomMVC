﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authentication.Google;
using EcomSiteMVC.Extensions.EmailService.Model;
using EcomSiteMVC.Core.IServices;
using EcomSiteMVC.Core.DTOs;
using EcomSiteMVC.Utilities;
using EcomSiteMVC.Extensions.EmailService.Service;


namespace EcomSiteMVC.Web.Controllers
{
    public class AuthController(IAuthService _authService, INotyfService _notyf, 
        IEmailService _emailService, IUserService _userService) : Controller
    {
        public IActionResult RegisterView()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterDTO model)
        {
            model.EmailVerificationToken = VerificationToken.GenerateEmailVerificationToken();

            var user = await _authService.Register(model, HttpContext.User);
            if (user != null)
            {
                var verificationLink = Url.Action("ConfirmEmail", "Auth", new { token = model.EmailVerificationToken, email = user.Email }, Request.Scheme);
                var emailMessage = new EmailMessage(new[] { user.Email }, "Please confirm your email", $"Please confirm your email by clicking the link: {verificationLink}");

                await _emailService.SendEmail(emailMessage);

                _notyf.Success("Register Successful", 5);
                return RedirectToAction("LoginView");
            }

            _notyf.Error("Error creating user, Username or Email already exists.", 5);
            return RedirectToAction("RegisterView");
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string token, string email)
        {
            var response = await _authService.ConfirmEmailVerification(token, email);
            if (response == true)
            {
                _notyf.Success("Email Verification Successful", 5);
                return RedirectToAction("LoginView");
            }
            _notyf.Error("Email Verification Failed", 5);
            return RedirectToAction("EmailVerificationFailed"); // TODO : PAGES NEEDS TO BE MADE FOR THESE
        }

        public IActionResult LoginView()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDTO model)
        {
            if (ModelState.IsValid)
            {
                var user = await _authService.CheckLogin(model);
                if (user != null)
                {
                    // Create the user claims
                    var claims = new List<Claim>
                    {
                        new Claim("UserId", user.UserId.ToString()),
                        new Claim(ClaimTypes.Name, user.Username),
                        new Claim(ClaimTypes.Email, user.Email),
                        new Claim(ClaimTypes.Role, user.Role.ToString())
                    };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var authProperties = new AuthenticationProperties
                    {
                        IsPersistent = model.RememberMe, // Make the login persistent according to RememberMe attribute

                        // Set the expiration time for the cookie according to "Remember Me" boolean
                        ExpiresUtc = model.RememberMe ? DateTimeOffset.UtcNow.AddDays(30) : DateTimeOffset.UtcNow.AddMinutes(30)
                    };

                    // Sign in the user
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

                    _notyf.Success("Login Successful", 5);
                    return RedirectToAction("ProfileView", "User");
                }
            }
            _notyf.Error("Invalid Email or Password. OR Verify Email.");
            return RedirectToAction("LoginView");
        }

        #region Google Auth 
        public IActionResult GoogleLoginPage()
        {
            // GoogleLoginCallback() url.
            var redirectUrl = Url.Action("GoogleLoginCallback");
            var properties = new AuthenticationProperties { RedirectUri = redirectUrl };
            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }

        public async Task<IActionResult> GoogleLoginCallback()
        {
            var result = await HttpContext.AuthenticateAsync("Google");
            if (result.Succeeded)
            {
                var claims = result?.Principal?.Identities?
                    .FirstOrDefault()?
                    .Claims
                    .Select(claim => new
                    {
                        claim.Issuer,
                        claim.OriginalIssuer,
                        claim.Type,
                        claim.Value
                    });

                var email = claims?.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value.ToLower();
                var googleUserId = result?.Principal?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var username = email?.Split('@')[0];

                // Create new user if user is not in database, and return the added user's data
                // If user exists, get the existing user data
                var user = await _authService.AuthFromGoogle(email, googleUserId);

                if (user == null) // user already exists, but must login from default login section, not google
                {
                    _notyf.Information("You are registered without google login, use normal login page to login.", 5);
                    return RedirectToAction("LoginView");
                }

                var claimsList = new List<Claim>
                {
                    new Claim("UserId", user.UserId.ToString()),
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Role, user.Role.ToString())
                };

                var claimsIdentity = new ClaimsIdentity(claimsList, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true,  // Always persist Google login
                    ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30)
                };

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

                _notyf.Success("Login Successful", 5);
                return RedirectToAction("ProfileView", "User");
            }
            _notyf.Error("Google login failed.", 5);
            return RedirectToAction("LoginView");
        }
        #endregion

        #region Forgot Password/ Reset
        public IActionResult ForgotPasswordView()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SendForgotPasswordLink(ResetPasswordDTO model)
        {
            var existingUser = await _userService.GetUserByEmail(model.Email);
            if (existingUser != null)
            {
                existingUser.PasswordResetToken = VerificationToken.GeneratePasswordResetToken();
                await _userService.UpdateUser(existingUser);
                var pwResetLink = Url.Action("ValidatePasswordResetToken", "Auth", new { token = existingUser.PasswordResetToken, email = existingUser.Email }, Request.Scheme);
                var emailMessage = new EmailMessage(new[] { existingUser.Email }, "Password reset link", $"Click the link to reset password: {pwResetLink}");

                await _emailService.SendEmail(emailMessage);

                _notyf.Success("Password Reset Link Sent to Email", 5);
                return RedirectToAction("LoginView");
            }
            _notyf.Error("User does not exists", 5);
            return Redirect(Request.Headers["Referer".ToString() ?? "/"]); // return the page where the user currently is
        }

        [HttpGet]
        public async Task<IActionResult> ValidatePasswordResetToken(string token, string email)
        {
            var response = await _authService.VerifyPasswordResetLink(token, email);
            if (response == true)
            {
                return RedirectToAction("CreateNewPasswordView", new { Token = token, Email = email });
            }
            _notyf.Error("Password Reset Link not Valid", 5);
            return RedirectToAction("LoginView");
        }

        public async Task<IActionResult> CreateNewPasswordView(string token, string? email)
        {
            // this is called so that CreateNewPasswordView cannot be called if the token is already used
            var response = await _authService.VerifyPasswordResetLink(token, email);

            if (!response)
            {
                _notyf.Error("Invalid Reset Password Token");
                return RedirectToAction("LoginView");
            }

            var model = new NewPasswordFromResetDTO
            {
                Email = email
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(NewPasswordFromResetDTO model)
        {
            var response = await _authService.ResetPassword(model);

            if (response == true)
            {
                _notyf.Success("Password Reset Successful", 5);
                return RedirectToAction("LoginView");
            }
            _notyf.Error("Password Reset Failed", 5);
            return RedirectToAction("LoginView");
        }
        #endregion

        public IActionResult Logout()
        {
            var logoutResult = HttpContext.SignOutAsync();
            if (logoutResult.IsCompleted)
            {
                _notyf.Success("Logout successful", 5);
                return RedirectToAction("LoginView");
            }
            _notyf.Error("Logout not successful", 5);
            return Redirect(Request.Headers["Referer".ToString() ?? "/"]); // return the page where the user currently is
        }
    }
}
