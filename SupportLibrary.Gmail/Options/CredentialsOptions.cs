using SupportLibrary.Common.Utils.Extensions;

namespace SupportLibrary.Gmail.Options
{
    public class CredentialsOptions
    {
        public CredentialsOptions() { }
        
        public CredentialsOptions(string privateKey, string clientEmail, string applicationEmail, string clientName = null)
        {
            PrivateKey = privateKey;
            ClientEmail = clientEmail;
            ApplicationEmail = applicationEmail;
            ClientName = clientName is not null && !clientName.IsEmptyOrWhiteSpace() ? clientName : "AutoGmailer";
        }

        public string PrivateKey { get; set; }
        public string ClientEmail { get; set; }
        public string ApplicationEmail { get; set; }
        public string ClientName { get; set; }
    }
}