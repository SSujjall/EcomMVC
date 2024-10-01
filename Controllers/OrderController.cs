using Microsoft.AspNetCore.Mvc;

namespace EcomSiteMVC.Controllers
{
    public class OrderController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
