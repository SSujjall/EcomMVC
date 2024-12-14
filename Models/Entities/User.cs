using EcomSiteMVC.Models.Enums;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace EcomSiteMVC.Models.Entities
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string? PasswordHash { get; set; }
        public string? GoogleUserId { get; set; }

        public Role Role { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateOnly CreatedDate { get; set; } = DateOnly.FromDateTime(DateTime.Now);
        public bool IsActive { get; set; }

        //Navigation Property
        public virtual UserProfile UserProfile { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        public virtual Cart Cart { get; set; }
    }
}