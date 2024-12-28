using EcomSiteMVC.Core.DTOs;
using EcomSiteMVC.Core.Models.Entities;
using System.Security.Claims;

namespace EcomSiteMVC.Core.IServices
{
    public interface IAuthService
    {
        Task<User> Register(RegisterDTO model, ClaimsPrincipal currentUser);
        Task<User?> CheckLogin(LoginDTO model);
        Task<User?> AuthFromGoogle(string email, string googleUserId);
        Task<bool> ConfirmEmailVerification(string token, string email);

    }
}
