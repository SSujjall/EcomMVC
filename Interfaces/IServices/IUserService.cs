using EcomSiteMVC.Models.DTOs;
using EcomSiteMVC.Models.Entities;

namespace EcomSiteMVC.Interfaces.IServices
{
    public interface IUserService
    {
        Task<UserDTO> GetExistingUserProfileAsync(int userId);
        Task<bool> CreateUserProfileAsync(UserDTO model, int userId);
        Task<bool> UpdateUserProfileAsync(UserDTO model, int userId);
    }
}