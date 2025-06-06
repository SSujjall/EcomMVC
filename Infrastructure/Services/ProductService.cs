﻿using EcomSiteMVC.Core.DTOs;
using EcomSiteMVC.Core.Enums;
using EcomSiteMVC.Core.IRepositories;
using EcomSiteMVC.Core.IServices;
using EcomSiteMVC.Core.Models.Entities;
using EcomSiteMVC.Core.Models.Helper;
using EcomSiteMVC.Utilities.ExternalServices.CloudinaryService.Service;

namespace EcomSiteMVC.Infrastructure.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly ICloudinaryService _cloudinaryService;
        private readonly string productPhotosFolder = FolderName.Ecom.ToString();

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
                }
                return true;
            }

            return false;
        }

        public async Task<IEnumerable<Product>> GetAllProduct()
        {
            return await _productRepository.GetAllAsync();
        }

        public async Task<IEnumerable<Product>> GetFilteredProducts(string? searchFilter, FilterModel filterModel)
        {
            var products = await _productRepository.GetAllAsync();

            if (!string.IsNullOrEmpty(searchFilter))
            {
                products = products.Where(p => p.ProductName.Contains(searchFilter, StringComparison.OrdinalIgnoreCase) ||
                                                           p.Description.Contains(searchFilter, StringComparison.OrdinalIgnoreCase));
            }
            if (!string.IsNullOrEmpty(filterModel.Category) && int.TryParse(filterModel.Category, out int categoryId))
            {
                products = products.Where(p => p.CategoryId == categoryId);
            }

            if (!string.IsNullOrEmpty(filterModel.MinPrice) && decimal.TryParse(filterModel.MinPrice, out decimal minPrice))
            {
                products = products.Where(p => p.Price >= minPrice);
            }

            if (!string.IsNullOrEmpty(filterModel.MaxPrice) && decimal.TryParse(filterModel.MaxPrice, out decimal maxPrice))
            {
                products = products.Where(p => p.Price <= maxPrice);
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

                if (model.Images != null)
                {
                    foreach (var item in model.Images)
                    {
                        var imageUrl = await _cloudinaryService.UploadImageAsync(item, FolderName.Ecom);
                        var productImage = new ProductImage
                        {
                            ProductId = product.ProductId,
                            ImageUrl = imageUrl
                        };
                        await _productRepository.AddImage(productImage);
                    }
                }
                product.LastUpdatedDate = DateOnly.FromDateTime(DateTime.Now);

                await _productRepository.Update(product);
                return true;
            }
            return false;
        }

        public async Task<bool> DeleteProduct(int id)
        {
            var product = await _productRepository.GetById(id);
            if (product != null)
            {
                foreach (var item in product.Images)
                {
                    var existingPublicId = item.ImageUrl.Split('/').Last().Split('.').First();
                    await _cloudinaryService.DeleteImageAsync($"{productPhotosFolder}/{existingPublicId}");
                }
                await _productRepository.Delete(product);
                return true;
            }
            return false;
        }

        public async Task<IEnumerable<Product>> PopularProducts(int TopN)
        {
            var products = await _productRepository.FilterPopularProducts(TopN);

            if (products == null || !products.Any())
            {
                return null;
            }
            return products;
        }

        public async Task<bool> DeleteProductImage(string imageUrl)
        {
            var imagePublicId = imageUrl.Split('/').Last().Split('.').First();
            await _cloudinaryService.DeleteImageAsync($"{productPhotosFolder}/{imagePublicId}");
            return true;
        }
    }
}
