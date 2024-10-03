using EcomSiteMVC.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EcomSiteMVC.Controllers
{
    public class ProductController : Controller
    {
        public IActionResult ProductView()
        {
            return View();
        }

        public IActionResult GetAllProducts()
        {
            return View();
        }

        public IActionResult GetProductById(int id)
        {
            return View();
        }

        //Admin Methods
        [Authorize(Roles = "Admin")]
        public IActionResult AddProduct(AddProductDTO model)
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        public IActionResult EditProduct(EditProductDTO model)
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        public IActionResult DeleteProduct(int id)
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        public IActionResult UpdateProduct(UpdateProductDTO model)
        {
            return View();
        }
    }
}
