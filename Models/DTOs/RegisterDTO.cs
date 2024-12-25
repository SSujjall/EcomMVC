using EcomSiteMVC.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace EcomSiteMVC.Models.DTOs
{
    public class RegisterDTO
    {
        [Required(ErrorMessage = "Username is required")]
        [StringLength(50, ErrorMessage = "Username cannot exceed 50 characters")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [MinLength(3, ErrorMessage = "Password must be atleast 3 characters.")]
        public string PasswordHash { get; set; }

        public string EmailVerificationToken { get; set; }
    }
}