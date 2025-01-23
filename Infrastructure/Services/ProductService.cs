using EcomSiteMVC.Core.DTOs;
using EcomSiteMVC.Core.IRepositories;
using EcomSiteMVC.Core.IServices;
using EcomSiteMVC.Core.Models.Entities;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using EcomSiteMVC.Core.Enums;

namespace EcomSiteMVC.Infrastructure.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly ICloudinaryService _cloudinaryService;


        public ProductService(IProductRepository productRepository, ICloudinaryService cloudinaryService)
        {
            _productRepository = productRepository;
            _cloudinaryService = cloudinaryService;
        }

        public async Task<bool> AddProduct(AddProductDTO model)
        {
            if (model != null)
            {
                var product = new Product
                {
                    CategoryId = model.CategoryId,
                    ProductName = model.ProductName,
                    Description = model.Description,
                    Price = model.Price,
                    StockQuantity = model.StockQuantity,
                    LastUpdatedDate = DateOnly.FromDateTime(DateTime.Now)
                };

                await _productRepository.Add(product);

                /* TODO: esko satta "var upImage = await _cloudinaryService.UploadMultipleImageAsync(model.Images, FolderName.Ecom);"
                         yo implement garnu parne xa for uploading multiple images.
                */
                if (model.Images != null)
                {
                    foreach (var image in model.Images)
                    {
                        var imageUrl = await _cloudinaryService.UploadImageAsync(image, FolderName.Ecom);
                        if (imageUrl != null)
                        {
                            var productImage = new ProductImage
                            {
                                ProductId = product.ProductId,
                                ImageUrl = imageUrl
                            };
                            await _productRepository.AddImage(productImage);
                        }
                    }
                    return true;
                }
            }

            return false;
        }

        public async Task<bool> DeleteProduct(int id)
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

        public async Task<IEnumerable<Product>> GetFilteredProducts(string? searchFilter)
        {
            var products = await _productRepository.GetAllAsync();

            if (!string.IsNullOrEmpty(searchFilter))
            {
                var filteredProducts = products.Where(p => p.ProductName.Contains(searchFilter, StringComparison.OrdinalIgnoreCase) ||
                                                           p.Description.Contains(searchFilter, StringComparison.OrdinalIgnoreCase));
                return filteredProducts;
            }
            return products;
        }

        public async Task<Product> GetProductById(int id)
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
                //product.ImageUrl = model.ImageUrl;
                product.LastUpdatedDate = DateOnly.FromDateTime(DateTime.Now);

                await _productRepository.Update(product);
                return true;
            }
            return false;
        }
    }
}
