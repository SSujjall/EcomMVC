using EcomSiteMVC.Interfaces.IRepositories;
using EcomSiteMVC.Interfaces.IServices;
using EcomSiteMVC.Models.Entities;

namespace EcomSiteMVC.Data.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<bool> AddCategory(Category category)
        {
            if (category != null)
            {
                await _categoryRepository.Add(category);
                return true;
            }
            return false;
        }

        public async Task<bool> UpdateCategory(Category category)
        {
            var existingCategory = await _categoryRepository.GetById(category.CategoryId);
            if (existingCategory != null)
            {
                existingCategory.CategoryName = category.CategoryName;
                existingCategory.Description = category.Description;
                await _categoryRepository.Update(existingCategory);
                return true;
            }
            return false;
        }

        public async Task<bool> DeleteCategory(int categoryId)
        {
            var category = await _categoryRepository.GetById(categoryId);
            if (category != null)
            {
                await _categoryRepository.Delete(category);
                return true;
            }
            return false;
        }

        public async Task<Category> GetCategoryById(int categoryId)
        {
            return await _categoryRepository.GetById(categoryId);
        }

        public async Task<IEnumerable<Category>> GetAllCategories()
        {
            return await _categoryRepository.GetAllAsync();
        }
    }
}
