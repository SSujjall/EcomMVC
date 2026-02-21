using EcomSiteMVC.Core.DTOs;
using EcomSiteMVC.Core.Models.ViewModels;

namespace EcomSiteMVC.Core.IServices
{
    public interface ICartService
    {
        public Task<bool> AddToCart(AddToCartDTO model);
        public Task<CartViewModel> GetCartByUserIdAsync(int userId);
        public Task<bool> DeleteCartItem(int id);
        public Task<bool> AddQuantity(int id);
        public Task<bool> SubstractQuantity(int id);
        public Task ClearCart(int userId);
    }
}
