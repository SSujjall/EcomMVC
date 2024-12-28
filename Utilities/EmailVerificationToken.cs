using System.Text;
using Newtonsoft.Json.Linq;

namespace EcomSiteMVC.Utilities
{
    public class EmailVerificationToken
    {
        public static string GenerateEmailVerificationToken()
        {
            var guid = Guid.NewGuid().ToString();
            var token = Convert.ToBase64String(Encoding.UTF8.GetBytes(guid)).ToString();
            return token;
        }

        public static bool VerifyEmailToken(string inputToken, string storedToken)
        {
            var decodedToken = Encoding.UTF8.GetString(Convert.FromBase64String(inputToken));
            return decodedToken == storedToken;
        }
    }
}
