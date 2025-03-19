using EcomSiteMVC.Core.DTOs;
using EcomSiteMVC.Core.IRepositories;
using EcomSiteMVC.Core.IServices;
using EcomSiteMVC.Core.Models.Entities;
using EcomSiteMVC.Utilities.Helpers;
using EcomSiteMVC.Utilities;

namespace EcomSiteMVC.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<UserDTO> GetExistingUserProfileAsync(int userId)
        {
            var returnObject = new UserDTO();

            var user = await _userRepository.GetUserAndProfileByUserIdAsync(userId);
            if (user != null)
            {
                returnObject.Username = user.Username;
                returnObject.Email = user.Email;

                if (user.UserProfile != null)
                {
                    returnObject.UserProfile = new UserProfile
                    {
                        FirstName = user.UserProfile.FirstName,
                        LastName = user.UserProfile.LastName,
                        PhoneNumber = user.UserProfile.PhoneNumber,
                        Address = user.UserProfile.Address,
                        DateOfBirth = user.UserProfile.DateOfBirth,
                        Gender = user.UserProfile.Gender,
                        ProfileImage = user.UserProfile.ProfileImage,
                    };
                }
                return returnObject;
            }
            return null; // return a new UserProfileUpdateDTO if you prefer empty fields
        }

        public async Task<bool> CreateUserProfileAsync(UserDTO model, int userId)
        {
            var existingUserProfile = await _userRepository.GetUserAndProfileByUserIdAsync(userId);

            if (existingUserProfile != null)
            {
                var result = await UpdateUserProfileAsync(model, userId);
                return result;
            }

            var userProfile = new User
            {
                UserProfile = new UserProfile
                {
                    UserId = userId,
                    FirstName = model.UserProfile.FirstName,
                    LastName = model.UserProfile.LastName,
                    PhoneNumber = model.UserProfile.PhoneNumber,
                    Address = model.UserProfile.Address,
                    DateOfBirth = model.UserProfile.DateOfBirth,
                    Gender = model.UserProfile.Gender,
                    ProfileImage = model.UserProfile.ProfileImage,
                }
            };

            await _userRepository.Add(userProfile);
            return true;
        }

        public async Task<bool> UpdateUserProfileAsync(UserDTO model, int userId)
        {
            var user = await _userRepository.GetUserAndProfileByUserIdAsync(userId);
            if (user != null)
            {
                user.Username = model.Username;
                user.Email = model.Email;
                user.UserProfile = new UserProfile
                {
                    FirstName = model.UserProfile.FirstName,
                    LastName = model.UserProfile.LastName,
                    PhoneNumber = model.UserProfile.PhoneNumber,
                    Address = model.UserProfile.Address,
                    DateOfBirth = model.UserProfile.DateOfBirth,
                    Gender = model.UserProfile.Gender,
                    ProfileImage = model.UserProfile.ProfileImage,
                };

                await _userRepository.Update(user);
                return true;
            }
            return false;
        }

        public async Task<User> GetUserByEmail(string email)
        {
            var user = await _userRepository.GetByUserEmailAsync(email);

            if (user != null)
            {
                return user;
            }
            return null;
        }

        public async Task<User> UpdateUser(User model)
        {
            if (model != null)
            {
                var updatedModel = await _userRepository.Update(model);
                return updatedModel;
            }
            return null;
        }

        public async Task<User> GetUserById(int id)
        {
            var user = await _userRepository.GetById(id);

            if (user != null)
            {
                return user;
            }
            return null;
        }

        public async Task<bool> CheckOldPwValidity(int userId, string oldPassword)
        {
            var user = await _userRepository.GetById(userId);
            if (user == null) return false;

            if (PasswordHelper.VerifyPassword(oldPassword, user.PasswordHash))
            {
                return true;
            }
            return false;
        }

        public async Task<bool> ChangeUserPassword(int userId, string newPassword)
        {
            var user = await _userRepository.GetById(userId);
            if (user == null) return false;

            // Hash new password and update
            user.PasswordHash = PasswordHelper.HashPassword(newPassword);
            user.PasswordChangeOTP = null; // Clear OTP after success
            await _userRepository.Update(user);

            return true;
        }
    }
}
