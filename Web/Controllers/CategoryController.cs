using AspNetCoreHero.ToastNotification.Abstractions;
using EcomSiteMVC.Core.DTOs;
using EcomSiteMVC.Core.IServices;
using EcomSiteMVC.Core.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EcomSiteMVC.Web.Controllers
{
    [Authorize(Roles = "Superadmin,Admin")]
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly INotyfService _notyf;

        public CategoryController(ICategoryService categoryService, INotyfService notyf)
        {
            _categoryService = categoryService;
            _notyf = notyf;
        }

        public async Task<IActionResult> CategoryViewPage()
        {
            var categoryList = await _categoryService.GetAllCategories();
            return View(categoryList);
        }

        public async Task<IActionResult> AddCategory(AddCategoryDTO model)
        {
            var result = await _categoryService.AddCategory(model);

            if (result.Item2 == true)
            {
                _notyf.Success(result.Item1, 5);
                return RedirectToAction("CategoryViewPage", result);
            }

            _notyf.Error(result.Item1, 5);
            return Redirect(Request.Headers["Referer".ToString() ?? "/"]);
        }

        public async Task<IActionResult> UpdateCategory(UpdateCategoryDTO model)
        {
            var categoryModel = new Category
            {
                CategoryId = model.CategoryId,
                CategoryName = model.CategoryName,
                Description = model.Description
            };

            var result = await _categoryService.UpdateCategory(categoryModel);

            if (result == true)
            {
                _notyf.Success("Category Updated", 5);
                return RedirectToAction("CategoryViewPage", result);
            }

            _notyf.Error("Error updating category", 5);
            return RedirectToAction("CategoryViewPage");
        }

        [Authorize(Roles = "Superadmin,Admin")]
        [HttpPost]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var result = await _categoryService.DeleteCategory(id);

            if (result)
            {
                _notyf.Success("Category deleted successfully!", 5);
                return Redirect(Request.Headers["Referer".ToString() ?? "/"]);
            }

            _notyf.Error("Failed to delete category!", 5);
            return Redirect(Request.Headers["Referer".ToString() ?? "/"]);
        }
    }
}
