using EcomSiteMVC.Core.DTOs;
using EcomSiteMVC.Core.Models.Entities;

namespace EcomSiteMVC.Core.IServices
{
    public interface IUserService
    {
        Task<UserDTO> GetExistingUserProfileAsync(int userId);
        Task<bool> CreateUserProfileAsync(UserDTO model, int userId);
        Task<bool> UpdateUserProfileAsync(UserDTO model, int userId);
        Task<User> GetUserByEmail(string email);
        Task<User> GetUserById(int id);
        Task<User> UpdateUser(User model);

        // TODO: Create a separate IOtpService for these methods. 'GenerateOtpForPasswordChange' and 'VerifyOtpForPasswordChange'
        // 'ChangeUserPassword' can be kept here as it is.
        Task<string> GenerateOtpForPasswordChange(int userId);
        Task<bool> VerifyOtpForPasswordChange(int userId, string otp);
        Task<bool> ChangeUserPassword(int userId, string oldPassword, string newPassword);
    }
}