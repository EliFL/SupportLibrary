namespace SupportLibrary.Gmail
{
    public interface IGmailClient
    {
        void SendMail(string mailSubject, string mailBody, string recipients, 
                      string ccRecipients = null, string pathToAttachment = null);
    }
}