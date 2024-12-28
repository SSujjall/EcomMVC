using Microsoft.AspNetCore.Mvc;

namespace EcomSiteMVC.Web.Controllers
{
    public class OrderController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
