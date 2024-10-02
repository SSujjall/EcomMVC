using EcomSiteMVC.Interfaces.IServices;
using EcomSiteMVC.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;


namespace EcomSiteMVC.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
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
                var user = await _authService.Register(model);
                if (user != null)
                {
                    TempData["ToastMessage"] = "Register successful!";
                    TempData["ToastType"] = "success";
                    return RedirectToAction("LoginView");
                }
            }
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
                var user = await _authService.Login(model);
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

                    TempData["ToastMessage"] = "Login successful!";
                    TempData["ToastType"] = "success";
                    return RedirectToAction("ProfileView", "User");
                }
            }

            TempData["ToastMessage"] = "Invalid username or password.";
            TempData["ToastType"] = "error";
            return RedirectToAction("LoginView");
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            TempData["ToastMessage"] = "Logout successful!";
            TempData["ToastType"] = "success";
            return RedirectToAction("LoginView");
        }

    }
}
