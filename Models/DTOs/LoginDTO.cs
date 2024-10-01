using EcomSiteMVC.Models.Enums;

namespace EcomSiteMVC.Models.DTOs
{
    public class LoginDTO
    {
        public string Username { get; set; }
        public string PasswordHash { get; set; }
    }
}
