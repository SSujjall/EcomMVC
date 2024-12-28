using EcomSiteMVC.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace EcomSiteMVC.Core.DTOs
{
    public class LoginDTO
    {
        [Required(ErrorMessage = "Username is required")]
        [StringLength(50, ErrorMessage = "Username cannot exceed 50 characters")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [MinLength(3, ErrorMessage = "Password must be atleast 3 characters")]
        public string PasswordHash { get; set; }

        public bool RememberMe { get; set; } = false;
    }
}
