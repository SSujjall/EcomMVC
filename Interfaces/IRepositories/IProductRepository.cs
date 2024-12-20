﻿using EcomSiteMVC.Models.Entities;

namespace EcomSiteMVC.Interfaces.IRepositories
{
    public interface IProductRepository : IRepositoryBase<Product>
    {
        Task AddImage(ProductImage image);
    }
}
