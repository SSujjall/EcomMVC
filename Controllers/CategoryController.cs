using Microsoft.AspNetCore.Mvc;

namespace EcomSiteMVC.Controllers
{
    public class CategoryController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
