using Microsoft.AspNetCore.Mvc;

namespace EcomSiteMVC.Controllers
{
    public class Product : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
