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

        public async Task<UserProfileUpdateDTO> GetUserProfileAsync(int userId)
        {
            var userProfile = await _userRepository.GetUserProfileByUserIdAsync(userId);
            if (userProfile != null)
            {
                return new UserProfileUpdateDTO
                {
                    FirstName = userProfile.FirstName,
                    LastName = userProfile.LastName,
                    PhoneNumber = userProfile.PhoneNumber,
                    Address = userProfile.Address,
                    DateOfBirth = userProfile.DateOfBirth,
                    Gender = userProfile.Gender,
                    ProfileImage = userProfile.ProfileImage,
                };
            }
            return null; // or return a new UserProfileUpdateDTO if you prefer empty fields
        }

        public async Task<bool> CreateUserProfileAsync(UserProfileUpdateDTO model, int userId)
        {
            var existingUserProfile = await _userRepository.GetUserProfileByUserIdAsync(userId);

            if (existingUserProfile != null)
            {
                var result = await UpdateUserProfileAsync(model, userId);
                return result;
            }

            var userProfile = new UserProfile
            {
                UserId = userId,
                FirstName = model.FirstName,
                LastName = model.LastName,
                PhoneNumber = model.PhoneNumber,
                Address = model.Address,
                DateOfBirth = model.DateOfBirth,
                Gender = model.Gender,
                ProfileImage = model.ProfileImage,
            };

            await _userRepository.Add(userProfile);
            return true;
        }

        public async Task<bool> UpdateUserProfileAsync(UserProfileUpdateDTO model, int userId)
        {
            var userProfile = await _userRepository.GetUserProfileByUserIdAsync(userId);
            if (userProfile != null)
            {
                userProfile.FirstName = model.FirstName;
                userProfile.LastName = model.LastName;
                userProfile.PhoneNumber = model.PhoneNumber;
                userProfile.Address = model.Address;
                userProfile.DateOfBirth = model.DateOfBirth;
                userProfile.Gender = model.Gender;
                userProfile.ProfileImage = model.ProfileImage;

                await _userRepository.Update(userProfile);
                return true;
            }
            return false;
        }
    }
}
