﻿using EcomSiteMVC.Core.DTOs;
using EcomSiteMVC.Core.Models.Entities;

namespace EcomSiteMVC.Core.IServices
{
    public interface IProductService
    {
        public Task<bool> AddProduct(AddProductDTO model);
        public Task<bool> UpdateProduct(UpdateProductDTO model);
        public Task<bool> DeleteProduct(int id);
        public Task<Product> GetProductById(int id);
        public Task<IEnumerable<Product>> GetAllProduct();
    }
}
