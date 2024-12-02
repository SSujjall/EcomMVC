using EcomSiteMVC.Models.DTOs;
using EcomSiteMVC.Models.Entities;
using System.Security.Claims;

namespace EcomSiteMVC.Interfaces.IServices
{
    public interface IAuthService
    {
        Task<User> Register(RegisterDTO model, ClaimsPrincipal currentUser);
        Task<User?> CheckLogin(LoginDTO model);
    }
}
