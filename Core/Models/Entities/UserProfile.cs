﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcomSiteMVC.Core.Models.Entities
{
    public class UserProfile
    {
        [Key]
        public int ProfileId { get; set; }
        [ForeignKey("User")]
        public int UserId { get; set; }
        public string ProfileImage { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateOnly DateOfBirth { get; set; }
        public string Gender { get; set; }

        // Navigation property
        public virtual User User { get; set; }
    }
}
