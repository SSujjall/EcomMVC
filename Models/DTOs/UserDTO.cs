using System.ComponentModel.DataAnnotations;
using EcomSiteMVC.Models.Entities;
using EcomSiteMVC.Models.Enums;

namespace EcomSiteMVC.Models.DTOs
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

    }
}