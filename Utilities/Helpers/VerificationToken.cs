using System.Text;
using Newtonsoft.Json.Linq;

namespace EcomSiteMVC.Utilities
{
    public class VerificationToken
    {

        public static string GenerateEmailVerificationToken()
        {
            var guid = Guid.NewGuid().ToString();
            var token = Convert.ToBase64String(Encoding.UTF8.GetBytes(guid)).ToString();
            return token;
        }

        public static bool VerifyEmailToken(string inputToken, string storedToken)
        {
            var decodedInputToken = Encoding.UTF8.GetString(Convert.FromBase64String(inputToken));
            var decodedStoredToken = Encoding.UTF8.GetString(Convert.FromBase64String(storedToken));
            return decodedInputToken == decodedStoredToken;
        }

        public static string GeneratePasswordResetToken()
        {
            var guid = Guid.NewGuid().ToString();
            var pwtoken = Convert.ToBase64String(Encoding.UTF8.GetBytes(guid)).ToString();
            return pwtoken;
        }

        public static bool VerifyPasswordResetToken(string inputToken, string storedToken)
        {
            var decodedInputToken = Encoding.UTF8.GetString(Convert.FromBase64String(inputToken));
            var decodedStoredToken = Encoding.UTF8.GetString(Convert.FromBase64String(storedToken));
            return decodedInputToken == decodedStoredToken;
        }
    }
}
