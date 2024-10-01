using Microsoft.AspNetCore.Mvc;

namespace EcomSiteMVC.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
