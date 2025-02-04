namespace EcomSiteMVC.Utilities.Helpers
{
    public static class EncryptDecryptHelper
    {
        private const string Salt = "1234567890"; // 10-digit salt

        public static string EncryptParameter(this string textToEncrypt)
        {
            if (string.IsNullOrEmpty(textToEncrypt))
                return string.Empty;

            // Append salt to text
            string saltedText = textToEncrypt + Salt;
            byte[] textBytes = System.Text.Encoding.UTF8.GetBytes(saltedText);
            string base64Encoded = Convert.ToBase64String(textBytes);

            // Remove trailing '=='
            return base64Encoded.TrimEnd('=');
        }

        public static string DecryptParameter(this string textToDecrypt)
        {
            if (string.IsNullOrEmpty(textToDecrypt))
                return string.Empty;

            try
            {
                // Pad with '=' to make it valid Base64
                int padding = textToDecrypt.Length % 4;
                if (padding > 0)
                {
                    textToDecrypt += new string('=', 4 - padding);
                }

                byte[] textBytes = Convert.FromBase64String(textToDecrypt);
                string decodedText = System.Text.Encoding.UTF8.GetString(textBytes);

                // Remove salt
                return decodedText.EndsWith(Salt) ? decodedText.Substring(0, decodedText.Length - Salt.Length) : string.Empty;
            }
            catch
            {
                return string.Empty;
            }
        }
    }
}
