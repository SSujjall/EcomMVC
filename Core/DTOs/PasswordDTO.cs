namespace EcomSiteMVC.Core.DTOs
{
    public class ResetPasswordDTO
    {
        public string Email { get; set; }
    }

    public class NewPasswordFromResetDTO
    {
        public string Email { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
    }

    public class ChangePasswordDTO
    {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
