using EcomSiteMVC.Models.DTOs;
using EcomSiteMVC.Models.Entities;

namespace EcomSiteMVC.Interfaces.IServices
{
    public interface IProductService
    {
        public Task<bool> AddProduct(AddProductDTO model);
        public Task<bool> UpdateProduct(UpdateProductDTO model);
        public Task<bool> DeleteProduct(string id);
        public Task<Product> GetProductById(string id);
        public Task<IEnumerable<Product>> GetAllProduct();
    }
}
