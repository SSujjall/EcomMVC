using EcomSiteMVC.Models.DTOs;
using EcomSiteMVC.Models.Entities;

namespace EcomSiteMVC.Interfaces.IServices
{
    public interface ICategoryService
    {
        Task<bool> AddCategory(AddCategoryDTO category);
        Task<bool> UpdateCategory(Category category);
        Task<bool> DeleteCategory(int categoryId);
        Task<Category> GetCategoryById(int categoryId);
        Task<IEnumerable<Category>> GetAllCategories();
    }
}
