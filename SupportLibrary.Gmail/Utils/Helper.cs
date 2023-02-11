using System.Net.Mail;

namespace SupportLibrary.Gmail.Utils
{
    internal static class Helper
    {
        internal static bool IsEmailValid(string email)
        {
            if (email.EndsWith(".")) { return false; }

            try
            {
                var mailAdress = new MailAddress(email);
                return mailAdress.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}