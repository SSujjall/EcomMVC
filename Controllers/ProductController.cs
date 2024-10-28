using EcomSiteMVC.Data.Services;
using EcomSiteMVC.Interfaces.IServices;
using EcomSiteMVC.Models.DTOs;
using EcomSiteMVC.Models.Enums;
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
        private readonly ICloudinaryService _cloudinaryService;

        public ProductController(IProductService productService, ICategoryService categoryService, ICloudinaryService cloudinaryService)
        {
            _productService = productService;
            _categoryService = categoryService;
            _cloudinaryService = cloudinaryService;
        }

        public async Task<IActionResult> CustomerProductView()
        {
            var categories = await _categoryService.GetAllCategories();
            ViewBag.CategoryList = new SelectList(categories, "CategoryId", "CategoryName");

            var products = await _productService.GetAllProduct();
            return View(products);
        }

        public IActionResult GetProductById(int id)
        {
            return View();
        }

        //Admin Methods
        [Authorize(Roles = "Superadmin,Admin")]
        public async Task<IActionResult> AllProductView()
        {
            var categories = await _categoryService.GetAllCategories();
            ViewBag.CategoryList = new SelectList(categories, "CategoryId", "CategoryName");

            var products = await _productService.GetAllProduct();
            return View(products);
        }

        [Authorize(Roles = "Superadmin,Admin")]
        public async Task<IActionResult> AddProductView()
        {
            var categories = await _categoryService.GetAllCategories();
            ViewBag.CategoryList = new SelectList(categories, "CategoryId", "CategoryName");
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Superadmin,Admin")]
        public async Task<IActionResult> AddProduct(AddProductDTO model, IFormFile productImage)
        {
            if (productImage != null && productImage.Length > 0)
            {
                var imageUrl = await _cloudinaryService.UploadImageAsync(productImage, FolderName.Ecom);
                if (imageUrl != null)
                {
                    model.ImageUrl = imageUrl;
                }
            }

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

            TempData["ToastMessage"] = "Failed to add product! Fill all the fields";
            TempData["ToastType"] = "error";
            return RedirectToAction("AddProductView", model);
        }

        [Authorize(Roles = "Superadmin,Admin")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var result = await _productService.DeleteProduct(id);

            if (result)
            {
                TempData["ToastMessage"] = "Product deleted successfully!";
                TempData["ToastType"] = "success";
                return RedirectToAction("AllProductView");
            }

            TempData["ToastMessage"] = "Failed to delete product!";
            TempData["ToastType"] = "error";
            return RedirectToAction("AllProductView");
        }

        [Authorize(Roles = "Superadmin,Admin")]
        public async Task<IActionResult> EditProductPage(int id)
        {
            var product = await _productService.GetProductById(id);

            return View("AddProductView", product);
        }

        [HttpPost]
        [Authorize(Roles = "Superadmin,Admin")]
        public IActionResult UpdateProduct(UpdateProductDTO model)
        {
            return View();
        }
    }
}