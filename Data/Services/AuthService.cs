﻿using EcomSiteMVC.Helpers;
using EcomSiteMVC.Interfaces.IRepositories;
using EcomSiteMVC.Interfaces.IServices;
using EcomSiteMVC.Models.DTOs;
using EcomSiteMVC.Models.Entities;
using EcomSiteMVC.Models.Enums;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text;

namespace EcomSiteMVC.Data.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;
        private readonly IRepositoryBase<User> _baseRepo;

        public AuthService(IAuthRepository authRepository, IRepositoryBase<User> baseRepo)
        {
            _authRepository = authRepository;
            _baseRepo = baseRepo;
        }

        public async Task<User?> CheckLogin(LoginDTO model)
        {
            // Username checking
            var req = new LoginDTO
            {
                Username = model.Username.ToLower(),
                PasswordHash = model.PasswordHash,
            };

            //check user using both username and email.
            var user = await _authRepository.GetUserByUsername(req.Username) ?? await _authRepository.GetUserByEmail(req.Username);

            // If user's email is not verified, don't let them sign in
            if (user.IsEmailVerified == false)
            {
                return null;
            }

            // check the user's role before login.
            var restrictedRoles = new HashSet<Role> { Role.Superadmin, Role.Admin };

            // check if user is admin. If the user isa admin ,dont let them sign in.
            // they have to use admin portal.
            if (user != null)
            {
                if (restrictedRoles.Contains(user.Role))
                {
                    return null;
                }
                if (user.GoogleUserId != null)// If user has signed up via Google
                {
                    //if google user tries to signin using a login portal then return null
                    //they should not be able to login through normal login portal
                    //they can only login using google singin
                    return null;
                }
                if (PasswordHelper.VerifyPassword(req.PasswordHash, user.PasswordHash))
                {
                    return user;
                }
            }
            return null;
        }

        public async Task<User> Register(RegisterDTO model, ClaimsPrincipal currentUser)
        {
            Role assignedRole = Role.User;

            // If the user is registering via Google, no password is needed. So checking if there is password or not
            if (model.PasswordHash != null)
            {
                // Setting role for the new user
                if (currentUser.Identity.IsAuthenticated)
                {
                    //Check if the current user is superadmin or not.
                    var roleClaim = currentUser.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role);

                    //If the current user is not superadmin, then set the registered user role to user.
                    if (roleClaim != null && Enum.TryParse(roleClaim.Value, out Role loggedInUserRole))
                    {
                        //If the current user is superadmin, then set the new registered user role to admin.
                        if (loggedInUserRole == Role.Superadmin)
                        {
                            assignedRole = Role.Admin;
                        }
                    }
                }

                var existingUser = await _authRepository.GetUserByUsername(model.Username);

                if (existingUser == null)
                {
                    var user = new User
                    {
                        Username = model.Username.ToLower(),
                        Email = model.Email.ToLower(),
                        PasswordHash = PasswordHelper.HashPassword(model.PasswordHash),
                        Role = assignedRole,
                        IsActive = true,
                        IsEmailVerified = false,
                        EmailVerificationToken = model.EmailVerificationToken
                    };

                    return await _authRepository.AddUser(user);
                }
            }
            return null;
        }

        // if user is not created then it will create user
        // if user already exists then it gives out the user.
        // No need to verify password like in the "CheckLogin" method as google auth users do not need password, it is handled by google itself.
        public async Task<User?> AuthFromGoogle(string email, string googleUserId)
        {
            var existingUser = await _authRepository.GetUserByEmail(email.ToLower());
            if (existingUser == null)
            {
                var user = new User
                {
                    Email = email.ToLower(),
                    Username = email.Split('@')[0], // Set username as email prefix
                    GoogleUserId = googleUserId, // Store Google User ID
                    Role = Role.User, // Default role or assign based on the current context
                    IsActive = true,
                    IsEmailVerified = true, // Verify the email by default for google login users
                };

                return await _authRepository.AddUser(user);
            }

            if (existingUser.GoogleUserId == googleUserId)
            {
                return existingUser; // Return the existing user if found
            }
            return null;
        }

        public async Task<bool> ConfirmEmailVerification(string token, string email)
        {
            var existingUser = await _authRepository.GetUserByEmail(email.ToLower());
            var decodedTokenFromURL = Encoding.UTF8.GetString(Convert.FromBase64String(token));
            var decodedTokenFromDB = Encoding.UTF8.GetString(Convert.FromBase64String(existingUser.EmailVerificationToken));
            if (existingUser != null && decodedTokenFromDB == decodedTokenFromURL)
            {
                existingUser.IsEmailVerified = true;
                await _baseRepo.Update(existingUser);
                return true;
            }
            return false;
        }
    }
}