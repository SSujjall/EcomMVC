namespace EcomSiteMVC.Utilities.Helpers
{
    public static class OtpHelper
    {
        public static string GeneratePasswordChangeOTP()
        {
            var rand = new Random();
            var otp = rand.Next(100000, 999999).ToString();
            return otp;
        }
    }
}
