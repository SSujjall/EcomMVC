using EcomSiteMVC.Models.DTOs;
using EcomSiteMVC.Models.Entities;

namespace EcomSiteMVC.Interfaces.IServices
{
    public interface IAuthService
    {
        Task<User> Register(RegisterDTO model);
        Task<User?> Login(LoginDTO model);
    }
}
