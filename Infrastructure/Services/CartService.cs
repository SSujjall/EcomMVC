using EcomSiteMVC.Core.DTOs;
using EcomSiteMVC.Core.IRepositories;
using EcomSiteMVC.Core.IServices;
using EcomSiteMVC.Core.Models.Entities;

namespace EcomSiteMVC.Infrastructure.Services
{
    public class CartService : ICartService
    {

        private readonly ICartRepository _cartRepository;
        private readonly IRepositoryBase<CartItem> _cartItemRepository;
        private readonly IRepositoryBase<Product> _productRepository;

        public CartService(
            ICartRepository cartRepository,
            IRepositoryBase<CartItem> cartItemRepository,
            IRepositoryBase<Product> productRepository)
        {
            _cartRepository = cartRepository;
            _cartItemRepository = cartItemRepository;
            _productRepository = productRepository;
        }

        public async Task<bool> AddToCart(AddToCartDTO model)
        {
            var product = await _productRepository.GetById(model.ProductId);
            if (product == null || product.StockQuantity < model.Quantity) return false;

            var cart = await _cartRepository.FindByConditionAsync(c => c.CustomerId == model.CustomerId)
                       ?? await _cartRepository.Add(new Cart { CustomerId = model.CustomerId, TotalAmount = 0 });

            var existingCartItem = await _cartItemRepository.FindByConditionAsync(ci => ci.CartId == cart.CartId && ci.ProductId == model.ProductId);
            if (existingCartItem != null)
            {
                existingCartItem.Quantity += model.Quantity;
                existingCartItem.UnitPrice = product.Price;
                await _cartItemRepository.Update(existingCartItem);
            }
            else
            {
                await _cartItemRepository.Add(new CartItem
                {
                    CartId = cart.CartId,
                    ProductId = model.ProductId,
                    Quantity = model.Quantity,
                    UnitPrice = product.Price
                });
            }

            var allCartItems = await _cartItemRepository.FindAllByConditionAsync(ci => ci.CartId == cart.CartId);
            cart.TotalAmount = allCartItems.Sum(item => item.Quantity * item.UnitPrice);

            await _cartRepository.Update(cart);

            return true;
        }

        public async Task<Cart> GetCartByUserIdAsync(int userId)
        {
            return await _cartRepository.FindByConditionAsync(c => c.CustomerId == userId);
        }
    }
}
