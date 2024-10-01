using EcomSiteMVC.Interfaces.IRepositories;
using EcomSiteMVC.Interfaces.IServices;
using EcomSiteMVC.Models.DTOs;
using EcomSiteMVC.Models.Entities;
using EcomSiteMVC.Models.Enums;
using Microsoft.AspNetCore.Authentication;

namespace EcomSiteMVC.Data.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;

        public AuthService(IAuthRepository authRepository)
        {
            _authRepository = authRepository;
        }

        public async Task<User?> Login(LoginDTO model)
        {
            var user = await _authRepository.GetUserByUsername(model.Username);
            if (user != null && VerifyPassword(model.PasswordHash, user.PasswordHash))
            {
                return user;
            }
            return null;
        }

        public async Task<User> Register(RegisterDTO model)
        {
            var user = new User
            {
                Username = model.Username,
                Email = model.Email,
                PasswordHash = HashPassword(model.PasswordHash),
                Role = Role.User,
                IsActive = true
            };

            return await _authRepository.AddUser(user);
        }

        private string HashPassword(string password)
        {
            return password;
        }

        private bool VerifyPassword(string enteredPassword, string storedPasswordHash)
        {
            return enteredPassword == storedPasswordHash;
        }
    }
}
