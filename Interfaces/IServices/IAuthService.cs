using EcomSiteMVC.Models.DTOs;
using EcomSiteMVC.Models.Entities;
using System.Security.Claims;

namespace EcomSiteMVC.Interfaces.IServices
{
    public interface IAuthService
    {
        Task<User> Register(RegisterDTO model, ClaimsPrincipal currentUser);
        Task<User?> CheckLogin(LoginDTO model);
        Task<User?> AuthFromGoogle(string email, string googleUserId);
        Task<bool> ConfirmEmailVerification(string token, string email);

    }
}
