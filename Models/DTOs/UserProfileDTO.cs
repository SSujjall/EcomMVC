﻿namespace EcomSiteMVC.Models.DTOs
{
    public class UserProfileUpdateDTO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public string Gender { get; set; }
    }

    public class UserPasswordUpdateDTO
    {

    }
}