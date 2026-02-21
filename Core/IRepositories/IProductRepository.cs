using EcomSiteMVC.Core.Models.Entities;
using EcomSiteMVC.Core.Models.ViewModels;

namespace EcomSiteMVC.Core.IRepositories
{
    public interface IProductRepository : IRepositoryBase<Product>
    {
        Task AddImage(ProductImage image);
        Task<IEnumerable<Product>> FilterPopularProducts(int topN);
        Task<CartProductViewModel> GetProductWithImage(int id);
    }
}
