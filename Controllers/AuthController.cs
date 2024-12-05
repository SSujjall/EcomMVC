using EcomSiteMVC.Interfaces.IServices;
using EcomSiteMVC.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using EcomSiteMVC.Models.Enums;
using AspNetCoreHero.ToastNotification.Abstractions;


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
                        IsPersistent = true, // Make the login persistent
                        ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30) // Set the expiration time for the cookie
                    };

                    // Sign in the user
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

                    _notyf.Success("Login Successful", 5);
                    return RedirectToAction("ProfileView", "User");
                }
            }
            _notyf.Error("Invalid username or password.");
            return RedirectToAction("LoginView");
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
