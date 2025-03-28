﻿using EcomSiteMVC.Core.Models.Entities;

namespace EcomSiteMVC.Core.IRepositories
{
    public interface IProductRepository : IRepositoryBase<Product>
    {
        Task AddImage(ProductImage image);
        Task<IEnumerable<Product>> FilterPopularProducts(int topN);
    }
}
