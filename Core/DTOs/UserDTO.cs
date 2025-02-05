using System.ComponentModel.DataAnnotations;
using EcomSiteMVC.Core.Enums;
using EcomSiteMVC.Core.Models.Entities;

namespace EcomSiteMVC.Core.DTOs
{
    public class UserDTO
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string? PasswordHash { get; set; }
        public Role Role { get; set; }
        public UserProfile UserProfile { get; set; }
    }

    public class UserPasswordUpdateDTO
    {
        public string UserId { get; set; }
    }
}