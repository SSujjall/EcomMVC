﻿using EcomSiteMVC.Interfaces.IServices;
using EcomSiteMVC.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using EcomSiteMVC.Models.Enums;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authentication.Google;


namespace EcomSiteMVC.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        private readonly INotyfService _notyf;

        public AuthController(IAuthService authService, INotyfService notyf)
        {
            _authService = authService;
            _notyf = notyf;
        }

        [HttpGet]
        public IActionResult NotFound()
        {
            return View();
        }

        public IActionResult RegisterView()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterDTO model)
        {
            if (ModelState.IsValid)
            {
                var user = await _authService.Register(model, HttpContext.User);
                if (user != null)
                {
                    _notyf.Success("Register Successful", 5);
                    return RedirectToAction("LoginView");
                }
            }
            _notyf.Error("Error creating user, try another Username.", 5);
            return RedirectToAction("RegisterView");
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

                        // Set the expiration time for the cookie
                        ExpiresUtc = model.RememberMe ? DateTimeOffset.UtcNow.AddDays(30) : DateTimeOffset.UtcNow.AddMinutes(30)
                    };

                    // Sign in the user
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

                    _notyf.Success("Login Successful", 5);
                    return RedirectToAction("ProfileView", "User");
                }
            }
            _notyf.Error("Invalid Email or Password");
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

                // Create new user if user is not in database, and give the added user data
                // If user exists, get the existing user data
                var user = await _authService.AuthFromGoogle(email, googleUserId);

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

        public IActionResult ForgotPasswordView()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ForgotPassword(string email)
        {
            return null;
        }

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
