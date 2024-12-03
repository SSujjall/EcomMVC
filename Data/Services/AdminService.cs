using EcomSiteMVC.Data.Repositories;
using EcomSiteMVC.Helpers;
using EcomSiteMVC.Interfaces.IRepositories;
using EcomSiteMVC.Interfaces.IServices;
using EcomSiteMVC.Models.DTOs;
using EcomSiteMVC.Models.Entities;
using EcomSiteMVC.Models.Enums;
using System.Linq;

namespace EcomSiteMVC.Data.Services
{
    public class AdminService : IAdminService
    {
        private readonly IAdminRepository _adminRepository;
        private readonly IAuthRepository _authRepository;

        public AdminService(IAdminRepository adminRepository, IAuthRepository authRepository)
        {
            _adminRepository = adminRepository;
            _authRepository = authRepository;
        }

        public async Task<IEnumerable<User>> GetAdminUsers()
        {
            var users = await _adminRepository.GetAllAsync();
            var adminUsers = users.Where(u => u.Role == Models.Enums.Role.Admin);
            return adminUsers;
        }

        public async Task<User?> LoginAdminUser(LoginDTO model)
        {
            // Username checking
            var req = new LoginDTO
            {
                Username = model.Username.ToLower(),
                PasswordHash = model.PasswordHash,
            };

            var user = await _authRepository.GetUserByUsername(req.Username);

            // check the user's role before login.
            var restrictedRoles = new HashSet<Role> { Role.User };

            // check if user is admin. If the user is not admin ,dont let them sign in.
            // they have to use normal user portal to sign in.
            if (user != null && restrictedRoles.Contains(user.Role))
            {
                return null;
            }

            if (user != null && PasswordHelper.VerifyPassword(req.PasswordHash, user.PasswordHash))
            {
                return user;
            }
            return null;
        }

        public async Task<bool> DeleteAdminUser(int id)
        {
            var result = await _adminRepository.GetById(id);
            if (result != null)
            {
                await _adminRepository.Delete(result);
                return true;
            }
            return false;
        }
    }
}