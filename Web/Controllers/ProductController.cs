using AspNetCoreHero.ToastNotification.Abstractions;
using EcomSiteMVC.Core.DTOs;
using EcomSiteMVC.Core.IServices;
using EcomSiteMVC.Core.Models.Entities;
using EcomSiteMVC.Core.Models.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EcomSiteMVC.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly INotyfService _notyf;

        public ProductController(IProductService productService, ICategoryService categoryService, INotyfService notyf)
        {
            _productService = productService;
            _categoryService = categoryService;
            _notyf = notyf;
        }

        public async Task<IActionResult> CustomerProductView(string? searchFilter, FilterModel filterModel)
        {
            var products = new List<Product>(); // sabai product load garera if ma janu and feri load vaisakeko products lai change garnu vanda yeuta var declare garera tesma changes garne.
            var categories = await _categoryService.GetAllCategories();
            ViewBag.CategoryList = new SelectList(categories, "CategoryId", "CategoryName");

            if (!string.IsNullOrEmpty(searchFilter) || filterModel != null)
            {
                products = (await _productService.GetFilteredProducts(searchFilter, filterModel)).ToList();
            }

            products = (await _productService.GetAllProduct()).ToList();

            if (!products.Any())
                _notyf.Warning("No products found.");

            return View(products);
        }

        public async Task<IActionResult> GetProductById(int id)
        {
            var product = await _productService.GetProductById(id);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        #region Admin Methods
        [Authorize(Roles = "Superadmin,Admin")]
        public async Task<IActionResult> AllProductView()
        {
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
        public async Task<IActionResult> AddProduct(AddProductDTO model)
        {
            if (ModelState.IsValid)
            {
                var result = await _productService.AddProduct(model);
                if (result)
                {
                    _notyf.Success("Product added successfully!", 5);
                    return RedirectToAction("AddProductView");
                }
            }

            _notyf.Error("Failed to add product! Fill all the fields", 5);
            return RedirectToAction("AddProductView", model);
        }

        [Authorize(Roles = "Superadmin,Admin")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var result = await _productService.DeleteProduct(id);

            if (result)
            {
                _notyf.Success("Product deleted successfully!", 5);
                return RedirectToAction("AllProductView");
            }

            _notyf.Error("Failed to delete product!", 5);
            return RedirectToAction("AllProductView");
        }

        [Authorize(Roles = "Superadmin,Admin")]
        public async Task<IActionResult> EditProductPage(int id)
        {
            var product = await _productService.GetProductById(id);

            var categories = await _categoryService.GetAllCategories();
            ViewBag.CategoryList = new SelectList(categories, "CategoryId", "CategoryName");

            return View("AddProductView", product);
        }

        [HttpPost]
        [Authorize(Roles = "Superadmin,Admin")]
        public async Task<IActionResult> UpdateProduct(UpdateProductDTO model)
        {
            if (ModelState.IsValid)
            {
                var result = await _productService.UpdateProduct(model);

                if (result)
                {
                    _notyf.Success("Product updated successfully!", 5);
                    return RedirectToAction("AllProductView");
                }

                _notyf.Error("Failed to update product!", 5);
                return RedirectToAction("AllProductView");
            }
            _notyf.Error("Model not valid! Please try again.", 5);
            return RedirectToAction("AllProductView");
        }
        #endregion
    }
}