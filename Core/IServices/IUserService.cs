using EcomSiteMVC.Core.DTOs;
using EcomSiteMVC.Core.Models.Entities;

namespace EcomSiteMVC.Core.IServices
{
    public interface IUserService
    {
        Task<UserDTO> GetExistingUserProfileAsync(int userId);
        Task<bool> CreateUserProfileAsync(UserDTO model, int userId);
        Task<bool> UpdateUserProfileAsync(UserDTO model, int userId);
    }
}