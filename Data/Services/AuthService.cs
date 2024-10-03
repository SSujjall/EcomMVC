using EcomSiteMVC.Interfaces.IRepositories;
using EcomSiteMVC.Interfaces.IServices;
using EcomSiteMVC.Models.DTOs;
using EcomSiteMVC.Models.Entities;
using EcomSiteMVC.Models.Enums;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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
            // Username checking
            var req = new LoginDTO
            {
                Username = model.Username.ToLower(),
                PasswordHash = model.PasswordHash,
            };

            var user = await _authRepository.GetUserByUsername(req.Username);
            if (user != null && VerifyPassword(req.PasswordHash, user.PasswordHash))
            {
                return user;
            }
            return null;
        }

        public async Task<User> Register(RegisterDTO model, ClaimsPrincipal currentUser)
        {
            Role assignedRole = Role.User;

            if (currentUser.Identity.IsAuthenticated)
            {
                var roleClaim = currentUser.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role);

                if (roleClaim != null && Enum.TryParse(roleClaim.Value, out Role loggedInUserRole))
                {
                    if (loggedInUserRole == Role.Superadmin)
                    {
                        assignedRole = Role.Admin;
                    }
                }
            }


            var user = new User
            {
                Username = model.Username.ToLower(),
                Email = model.Email.ToLower(),
                PasswordHash = HashPassword(model.PasswordHash),
                Role = assignedRole,
                IsActive = true
            };

            return await _authRepository.AddUser(user);
        }

        private string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        private bool VerifyPassword(string enteredPassword, string storedPasswordHash)
        {
            return BCrypt.Net.BCrypt.Verify(enteredPassword, storedPasswordHash);
        }
    }
}