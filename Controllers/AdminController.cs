using AspNetCoreHero.ToastNotification.Abstractions;
using EcomSiteMVC.Interfaces.IServices;
using EcomSiteMVC.Models.DTOs;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection.KeyManagement.Internal;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EcomSiteMVC.Controllers
{
    [Authorize(Roles = "Admin,Superadmin")]
    public class AdminController : Controller
    {
        private readonly IAuthService _authService;
        private readonly IAdminService _adminService;
        private readonly INotyfService _notyf;

        public AdminController(IAuthService authService, IAdminService adminService, INotyfService notyf)
        {
            _authService = authService;
            _adminService = adminService;
            _notyf = notyf;
        }

        [HttpGet("{controller}/dashboard")]
        public IActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpGet("{controller}/login:port")]
        public IActionResult AdminLoginView()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> AdminLogin(LoginDTO model)
        {
            if (ModelState.IsValid)
            {
                var user = await _adminService.LoginAdminUser(model);

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
                    return RedirectToAction("Index");
                }
            }
            _notyf.Error("Invalid username or password for admin.");
            return RedirectToAction("AdminLoginView");
        }

        [HttpGet("{controller}/add-admin")]
        public async Task<IActionResult> AddAdminView()
        {
            var adminList = await _adminService.GetAdminUsers();
            return View(adminList);
        }

        [Authorize(Roles = "Superadmin")] // Only superadmin can add new admins
        [HttpPost]
        public async Task<IActionResult> AddAdmin(RegisterDTO model)
        {
            if (ModelState.IsValid)
            {
                var user = await _authService.Register(model, HttpContext.User);
                if (user != null)
                {
                    _notyf.Success("Added New Admin!", 5);
                    return RedirectToAction("AddAdminView");
                }
            }

            _notyf.Error("Failed to add New Admin!", 5);
            return RedirectToAction("AddAdminView");
        }

        [Authorize(Roles = "Superadmin")] // Only superadmin can add new admins
        [HttpPost]
        public async Task<IActionResult> DeleteAdmin(int id)
        {
            if (ModelState.IsValid)
            {
                var user = await _adminService.DeleteAdminUser(id);
                if (user != null)
                {
                    _notyf.Success("Deleted Admin!", 5);
                    return RedirectToAction("AddAdminView");
                }
            }

            _notyf.Error("Failed to Delete Admin!", 5);
            return RedirectToAction("AddAdminView");
        }

    }
}
