using EcomSiteMVC.Models.Enums;

namespace EcomSiteMVC.Models.DTOs
{
    public class RegisterDTO
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
    }
}