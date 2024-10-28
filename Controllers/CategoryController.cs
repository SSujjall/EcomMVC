using AspNetCoreHero.ToastNotification.Abstractions;
using EcomSiteMVC.Interfaces.IServices;
using EcomSiteMVC.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EcomSiteMVC.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly INotyfService _notyf;

        public CategoryController(ICategoryService categoryService, INotyfService notyf)
        {
            _categoryService = categoryService;
            _notyf = notyf;
        }

        [Authorize(Roles = "Superadmin,admin")]
        public async Task<IActionResult> CategoryViewPage()
        {
            var categoryList = await _categoryService.GetAllCategories();
            return View(categoryList);
        }

        [Authorize(Roles = "Superadmin,admin")]
        public async Task<IActionResult> AddCategory(AddCategoryDTO model)
        {
            var result = await _categoryService.AddCategory(model);

            if (result == true)
            {
                _notyf.Success("Category Added",5);
                return RedirectToAction("CategoryViewPage", result);
            }

            _notyf.Error("Category not added", 5);
            return RedirectToAction("CategoryViewPage");
        }
    }
}
