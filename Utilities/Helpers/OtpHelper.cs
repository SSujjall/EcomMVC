using System;
using System.Text;

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

        public static bool VerifyPasswordResetToken(string inputOtp, string storedOtp)
        {
            if (string.IsNullOrEmpty(inputOtp) || string.IsNullOrEmpty(storedOtp))
            {
                return false; // Invalid if any token is missing
            }
            try
            {
                var decodedInputOtp = Encoding.UTF8.GetString(Convert.FromBase64String(inputOtp));
                var decodedStoredOtp = Encoding.UTF8.GetString(Convert.FromBase64String(storedOtp));
                return decodedInputOtp == decodedStoredOtp;
            }
            catch (FormatException)
            {
                return false; // Handle invalid Base64 strings
            }
        }
    }
}
