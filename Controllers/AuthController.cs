using Microsoft.AspNetCore.Mvc;

namespace EcomSiteMVC.Controllers
{
    public class AuthController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
