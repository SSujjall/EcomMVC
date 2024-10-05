using EcomSiteMVC.Interfaces.IServices;
using EcomSiteMVC.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace EcomSiteMVC.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;

        public ProductController(IProductService productService, ICategoryService categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
        }

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
        [Authorize(Roles = "Superadmin,Admin")]
        public async Task<IActionResult> AddProductView()
        {
            var categories = await _categoryService.GetAllCategories();
            ViewBag.CategoryList = new SelectList(categories, "CategoryId", "CategoryName");
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Superadmin,Admin")]
        public async Task<IActionResult> AddProduct(AddProductDTO model)
        {
            if (ModelState.IsValid)
            {
                var result = await _productService.AddProduct(model);
                if (result)
                {
                    TempData["ToastMessage"] = "Product added successfully!";
                    TempData["ToastType"] = "success";
                    return RedirectToAction("AddProductView");
                }
            }

            // If there is an error, reload categories and return to view
            var categories = await _categoryService.GetAllCategories();
            ViewBag.CategoryList = new SelectList(categories, "CategoryId", "CategoryName");
            return View(model);
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
