using EcomSiteMVC.Models.DTOs;
using EcomSiteMVC.Models.Entities;

namespace EcomSiteMVC.Interfaces.IServices
{
    public interface IUserService
    {
        Task<User> GetUserProfileAsync(int userId);
        Task<bool> CreateUserProfileAsync(User model, int userId);
        Task<bool> UpdateUserProfileAsync(User model, int userId);
    }
}