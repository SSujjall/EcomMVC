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

        public AdminController(IAuthService authService)
        {
            _authService = authService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("{controller}/add-admin")]
        public IActionResult AddAdminView()
        {
            return View();
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
                    TempData["ToastMessage"] = "Added New Admin!";
                    TempData["ToastType"] = "success";
                    return RedirectToAction("AddAdminView");
                }
            }
            TempData["ToastMessage"] = "Failed to add New Admin!";
            TempData["ToastType"] = "error";
            return RedirectToAction("AddAdminView");
        }

    }
}
