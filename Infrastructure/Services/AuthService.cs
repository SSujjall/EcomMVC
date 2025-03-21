using EcomSiteMVC.Core.DTOs;
using EcomSiteMVC.Core.Enums;
using EcomSiteMVC.Core.IRepositories;
using EcomSiteMVC.Core.IServices;
using EcomSiteMVC.Core.Models.Entities;
using EcomSiteMVC.Utilities;
using System.Security.Claims;

namespace EcomSiteMVC.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;
        private readonly IUserRepository _userRepository;

        public AuthService(IAuthRepository authRepository, IUserRepository userRepository)
        {
            _authRepository = authRepository;
            _userRepository = userRepository;
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

            // check the user's role before login.
            var restrictedRoles = new HashSet<Role> { Role.Superadmin, Role.Admin };

            // check if user is admin. If the user isa admin ,dont let them sign in.
            // they have to use admin portal.
            if (user != null)
            {
                // TODO: Need to revise the logic
                // The issue right now is that if a google account logged in user changes their password
                // they are able to change it but are not able to login with the password
                // because 'user.GoogleUserId != null' condition is checked before the password verification
                // and since google user has their googleuserId, they are not able to reach the pw verify condition.

                if (restrictedRoles.Contains(user.Role))
                {
                    return null;
                }
                else if (PasswordHelper.VerifyPassword(req.PasswordHash, user.PasswordHash))
                {
                    if (user.IsEmailVerified == false) // If user's email is not verified, don't let them sign in
                    {
                        return null;
                    }
                    return user;
                }
                else if (user.GoogleUserId != null)// If user has signed up via Google
                {
                    //if google user tries to signin using a login portal then return null
                    //they should not be able to login through normal login portal
                    //they can only login using google singin
                    return null;
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

                var existingUsername = await _authRepository.GetUserByUsername(model.Username);
                var existingEmail = await _authRepository.GetUserByEmail(model.Email);
                if (existingUsername == null && existingEmail == null)
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

        /** if user is not created then it will create user
            if user already exists then it gives out the user.
            No need to verify password like in the "CheckLogin" method as google auth users do not need password, it is handled by google itself.
        **/
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
            if (existingUser == null)
            {
                return false;
            }

            if (VerificationToken.VerifyEmailToken(token, existingUser?.EmailVerificationToken))
            {
                existingUser.IsEmailVerified = true;
                await _userRepository.Update(existingUser);
                return true;
            }
            return false;
        }

        #region Forgot Password Section
        public async Task<bool> VerifyPasswordResetLink(string token, string email)
        {
            var existingUser = await _authRepository.GetUserByEmail(email.ToLower());
            if (existingUser == null)
            {
                return false;
            }
            if (VerificationToken.VerifyPasswordResetToken(token, existingUser?.PasswordResetToken))
            {
                return true;
            }
            return false;
        }

        public async Task<bool> ResetPassword(NewPasswordFromResetDTO model)
        {
            var existingUser = await _authRepository.GetUserByEmail(model.Email.ToLower());
            if (existingUser == null)
            {
                return false;
            }

            if (model.NewPassword == model.ConfirmPassword)
            {
                existingUser.PasswordHash = PasswordHelper.HashPassword(model.ConfirmPassword);
                existingUser.PasswordResetToken = null;
                var result = await _userRepository.Update(existingUser);
                if (result == null)
                {
                    return false;
                }
                return true;
            }
            return false;
        }
        #endregion
    }
}