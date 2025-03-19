namespace EcomSiteMVC.Core.IServices
{
    public interface IOtpService
    {
        Task<string> GeneratePasswordChangeOtp(int userId);
        Task<bool> VerifyPasswordChangeOtp(int userId, string otp);
    }
}
