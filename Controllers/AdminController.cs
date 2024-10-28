using AspNetCoreHero.ToastNotification.Abstractions;
using EcomSiteMVC.Interfaces.IServices;
using EcomSiteMVC.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection.KeyManagement.Internal;
using Microsoft.AspNetCore.Mvc;

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

        public IActionResult Index()
        {
            return View();
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
    }
}
