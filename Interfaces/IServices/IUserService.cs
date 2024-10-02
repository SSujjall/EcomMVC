using EcomSiteMVC.Models.DTOs;
using EcomSiteMVC.Models.Entities;

namespace EcomSiteMVC.Interfaces.IServices
{
    public interface IUserService
    {
        Task<UserProfileUpdateDTO> GetUserProfileAsync(int userId);
        Task<bool> CreateUserProfileAsync(UserProfileUpdateDTO model, int userId);
        Task<bool> UpdateUserProfileAsync(UserProfileUpdateDTO model, int userId);
    }
}
