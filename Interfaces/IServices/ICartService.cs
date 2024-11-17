using EcomSiteMVC.Models.DTOs;
using EcomSiteMVC.Models.Entities;

namespace EcomSiteMVC.Interfaces.IServices
{
    public interface ICartService
    {
        public Task<bool> AddToCart(AddToCartDTO model);
        public Task<Cart> GetCartByUserIdAsync(int userId);
    }
}
