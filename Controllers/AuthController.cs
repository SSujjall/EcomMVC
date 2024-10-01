using EcomSiteMVC.Interfaces.IServices;
using EcomSiteMVC.Models.DTOs;
using Microsoft.AspNetCore.Mvc;

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
                    TempData["ToastMessage"] = "Login successful!";
                    TempData["ToastType"] = "success";
                    return RedirectToAction("Index", "Home");
                }
            }
            TempData["ToastMessage"] = "Invalid username or password.";
            TempData["ToastType"] = "error";
            return RedirectToAction("LoginView");
        }
    }
}
