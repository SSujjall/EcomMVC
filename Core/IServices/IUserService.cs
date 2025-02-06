using EcomSiteMVC.Core.DTOs;
using EcomSiteMVC.Core.Models.Entities;

namespace EcomSiteMVC.Core.IServices
{
    public interface IUserService
    {
        Task<UserDTO> GetExistingUserProfileAsync(int userId);
        Task<bool> CreateUserProfileAsync(UserDTO model, int userId);
        Task<bool> UpdateUserProfileAsync(UserDTO model, int userId);
        Task<User> GetUserByEmail(string email); //
        Task<User> GetUserById(int id); // Remove these 3 methods and create separate service to be called in controller.
        Task<User> UpdateUser(User model); //
        Task<string> GenerateOtpForPasswordChange(int userId);
        Task<bool> VerifyOtpForPasswordChange(int userId, string otp);
        Task<bool> ChangeUserPassword(int userId, string oldPassword, string newPassword);
    }
}