using Microsoft.AspNetCore.Mvc;

namespace EcomSiteMVC.Controllers
{
    public class CartController : Controller
    {
        public IActionResult CartView()
        {
            return View();
        }

        public IActionResult AddToCart()
        {
            return View();
        }
    }
}
