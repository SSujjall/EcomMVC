using System.ComponentModel.DataAnnotations;

namespace EcomSiteMVC.Models.DTOs
{
    public class UserProfileUpdateDTO
    {
        public string ProfileImage { get; set; }

        [Required(ErrorMessage = "FirstName Required")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "LastName Required")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "PhoneNumber Required")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Address Required")]
        public string Address { get; set; }

        [Required(ErrorMessage = "DateOfBirth Required")]
        public DateOnly DateOfBirth { get; set; }

        [Required(ErrorMessage = "Gender Required")]
        public string Gender { get; set; }
    }

    public class UserPasswordUpdateDTO
    {

    }
}