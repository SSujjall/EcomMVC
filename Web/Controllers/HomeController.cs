using EcomSiteMVC.Core.IServices;
using EcomSiteMVC.Core.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace EcomSiteMVC.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProductService _productService;

        public HomeController(ILogger<HomeController> logger, IProductService productService)
        {
            _logger = logger;
            _productService = productService;
        }

        public async Task<IActionResult> Index()
        {
            #region Get Popular Products
            var trendingProducts = await _productService.PopularProducts(5);
            #endregion

            return View(trendingProducts);
        }

        [Route("/NotFound")]
        public IActionResult PageNotFound()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
