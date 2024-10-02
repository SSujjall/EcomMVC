using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EcomSiteMVC.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("{controller}/add-admin")]
        public IActionResult AddAdminView()
        {
            return View();
        }

        public IActionResult AddAdmin()
        {
            return RedirectToAction("AddAdminView");
        }

    }
}
