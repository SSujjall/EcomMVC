using EcomSiteMVC.Core.DTOs;
using EcomSiteMVC.Core.IRepositories;
using EcomSiteMVC.Core.IServices;
using EcomSiteMVC.Core.Models.Entities;
using EcomSiteMVC.Core.Models.ViewModels;

namespace EcomSiteMVC.Infrastructure.Services
{
    public class CartService : ICartService
    {

        private readonly ICartRepository _cartRepository;
        private readonly IRepositoryBase<CartItem> _cartItemRepository;
        private readonly IProductRepository _productRepository;

        public CartService(
            ICartRepository cartRepository,
            IRepositoryBase<CartItem> cartItemRepository,
            IProductRepository productRepository)
        {
            _cartRepository = cartRepository;
            _cartItemRepository = cartItemRepository;
            _productRepository = productRepository;
        }

        public async Task<bool> AddToCart(AddToCartDTO model)
        {
            var product = await _productRepository.GetById(model.ProductId);
            if (product == null || product.StockQuantity < model.Quantity) return false;

            var cart = await _cartRepository.FindSingleByConditionAsync(c => c.CustomerId == model.CustomerId)
                       ?? await _cartRepository.Add(new Cart { CustomerId = model.CustomerId });

            var existingCartItem = await _cartItemRepository.FindSingleByConditionAsync(ci => ci.CartId == cart.CartId && ci.ProductId == model.ProductId);
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
            return true;
        }

        public async Task<CartViewModel> GetCartByUserIdAsync(int userId)
        {
            var cart = await _cartRepository.FindSingleByConditionAsync(c => c.CustomerId == userId);
            if (cart == null) return null;

            var cartVm = new CartViewModel
            {
                CustomerId = cart.CustomerId,
                CartItems = cart.CartItems.Select(ci => new CartItemViewModel
                {
                    CartItemId = ci.CartItemId,
                    ProductId = ci.ProductId,
                    Quantity = ci.Quantity,
                    UnitPrice = ci.UnitPrice,
                    Product = new CartProductViewModel
                    {
                        ProductId = ci.Product.ProductId,
                        ProductName = ci.Product.ProductName,
                        Price = ci.Product.Price,
                        Images = ci.Product.Images
                            .Select(img => new CartProductImageViewModel
                            {
                                ImageId = img.ImageId,
                                ImageUrl = img.ImageUrl
                            })
                            .ToList()
                    }
                }).ToList()
            };
            return cartVm;
        }

        public async Task<bool> DeleteCartItem(int id)
        {
            var existingCartItem = await _cartItemRepository.GetById(id);
            if (existingCartItem == null)
            {
                return false;
            }

            await _cartItemRepository.Delete(existingCartItem);
            return true;
        }

        public async Task<bool> AddQuantity(int id)
        {
            var existingCartItem = await _cartItemRepository.GetById(id);
            var existingProduct = await _productRepository.GetById(existingCartItem.ProductId);
            var isStockAvailable = existingProduct.StockQuantity - existingCartItem.Quantity != 0 ? true : false;

            if (existingCartItem != null && isStockAvailable)
            {
                existingCartItem.Quantity += 1;
                var res = await _cartItemRepository.Update(existingCartItem);

                if (res != null)
                    return true;
            }
            return false;
        }

        public async Task<bool> SubstractQuantity(int id)
        {
            var existingCartItem = await _cartItemRepository.GetById(id);
            if (existingCartItem != null)
            {
                existingCartItem.Quantity -= 1;

                if (existingCartItem.Quantity > 0)
                {
                    var res = await _cartItemRepository.Update(existingCartItem);
                    if (res != null)
                    {
                        return true;
                    }
                    return false;
                }
                else // Delete the cart item if the quantity is 0 or -ve
                {
                    await _cartItemRepository.Delete(existingCartItem);
                    return true;
                }
            }
            return false;
        }

        public async Task ClearCart(int userId)
        {
            var userCart = await _cartRepository.FindSingleByConditionAsync(x => x.CustomerId == userId);

            if (userCart != null && userCart.CartItems.Any())
            {
                for (int i = userCart.CartItems.Count - 1; i >= 0; i--)
                {
                    await _cartItemRepository.Delete(userCart.CartItems[i]);
                }
            }
        }
    }
}
