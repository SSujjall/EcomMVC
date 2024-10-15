using EcomSiteMVC.Interfaces.IServices;
using EcomSiteMVC.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EcomSiteMVC.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public async Task<IActionResult> CategoryViewPage()
        {
            var categoryList = await _categoryService.GetAllCategories();
            return View(categoryList);
        }

        [Authorize(Roles = "Superadmin,admin")]
        public async Task<IActionResult> AddCategory(AddCategoryDTO model)
        {
            var result = await _categoryService.AddCategory(model);
            return RedirectToAction("CategoryViewPage", result);
        }
    }
}
