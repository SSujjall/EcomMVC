using EcomSiteMVC.Interfaces.IRepositories;
using EcomSiteMVC.Interfaces.IServices;
using EcomSiteMVC.Models.DTOs;
using EcomSiteMVC.Models.Entities;

namespace EcomSiteMVC.Data.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<UserDTO> GetUserProfileAsync(int userId)
        {
            var user = await _userRepository.GetUserAndProfileByUserIdAsync(userId);
            if (user.UserProfile != null)
            {
                return new UserDTO
                {
                    Username = user.Username,
                    Email = user.Email,
                    UserProfile = new UserProfile
                    {
                        FirstName = user.UserProfile.FirstName,
                        LastName = user.UserProfile.LastName,
                        PhoneNumber = user.UserProfile.PhoneNumber,
                        Address = user.UserProfile.Address,
                        DateOfBirth = user.UserProfile.DateOfBirth,
                        Gender = user.UserProfile.Gender,
                        ProfileImage = user.UserProfile.ProfileImage,
                    }
                };
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
    }
}
