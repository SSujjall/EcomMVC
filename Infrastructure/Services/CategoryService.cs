﻿using EcomSiteMVC.Core.DTOs;
using EcomSiteMVC.Core.IRepositories;
using EcomSiteMVC.Core.IServices;
using EcomSiteMVC.Core.Models.Entities;

namespace EcomSiteMVC.Infrastructure.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<bool> AddCategory(AddCategoryDTO categoryDto)
        {
            var categoryModel = new Category
            {
                CategoryName = categoryDto.CategoryName,
                Description = categoryDto.Description
            };

            if (categoryDto != null)
            {
                await _categoryRepository.Add(categoryModel);
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
