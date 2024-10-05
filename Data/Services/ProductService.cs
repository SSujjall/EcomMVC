using EcomSiteMVC.Interfaces.IRepositories;
using EcomSiteMVC.Interfaces.IServices;
using EcomSiteMVC.Models.DTOs;
using EcomSiteMVC.Models.Entities;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace EcomSiteMVC.Data.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;

        public ProductService(IProductRepository productRepository, ICategoryRepository categoryRepository)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
        }

        public async Task<bool> AddProduct(AddProductDTO model)
        {
            if (model != null)
            {
                var data = new Product
                {
                    CategoryId = model.CategoryId,
                    ProductName = model.ProductName,
                    Description = model.Description,
                    Price = model.Price,
                    StockQuantity = model.StockQuantity,
                    ImageUrl = model.ImageUrl,
                    LastUpdatedDate = DateOnly.FromDateTime(DateTime.Now)
                };

                await _productRepository.Add(data);
                return true;
            }

            return false;
        }

        public async Task<bool> DeleteProduct(string id)
        {
            var product = await _productRepository.GetById(id);
            if (product != null)
            {
                await _productRepository.Delete(product);
                return true;
            }
            return false;
        }

        public async Task<IEnumerable<Product>> GetAllProduct()
        {
            return await _productRepository.GetAllAsync();
        }

        public async Task<Product> GetProductById(string id)
        {
            return await _productRepository.GetById(id);
        }

        public async Task<bool> UpdateProduct(UpdateProductDTO model)
        {
            var product = await _productRepository.GetById(model.ProductId);
            if (product != null)
            {
                product.ProductName = model.ProductName;
                product.Description = model.Description;
                product.Price = model.Price;
                product.StockQuantity = model.StockQuantity;
                product.ImageUrl = model.ImageUrl;
                product.LastUpdatedDate = DateOnly.FromDateTime(DateTime.Now);

                await _productRepository.Update(product);
                return true;
            }
            return false;
        }

        public async Task<IEnumerable<Category>> GetAllCategories()
        {
            return await _categoryRepository.GetAllAsync();
        }
    }
}
