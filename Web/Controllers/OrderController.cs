using EcomSiteMVC.Core.IServices;
using Microsoft.AspNetCore.Mvc;

namespace EcomSiteMVC.Web.Controllers
{
    public class OrderController(ICartService _cartService, IUserService _userService) : Controller
    {
        public async Task<IActionResult> OrderConfirmation()
        {
            var userId = int.Parse(User.FindFirst("UserId").Value);
            var cartProducts = await _cartService.GetCartByUserIdAsync(userId);
            var userDetails = await _userService.GetExistingUserProfileAsync(userId);

            ViewBag.UserDetail = userDetails;
            return View(cartProducts);
        }
    }
}
