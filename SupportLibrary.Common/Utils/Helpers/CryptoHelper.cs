using System.Text;    
using System.Security.Cryptography;
using SupportLibrary.Common.Utils.Extensions;

namespace SupportLibrary.Common.Utils.Helpers
{
    public static class CryptoHelper
    {
        public static string GetSha256Hash(string valueToHash)
        {
            StringHelper.ValidateInput(valueToHash);

            using (var sha256 = SHA256.Create())
            {
                return String.Join(String.Empty, sha256.ComputeHash(valueToHash.ToByteArray(Encoding.UTF8)).Select(item => item.ToString("x2")));
            }
        }
    }
}