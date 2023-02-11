using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;
using Google.Apis.Gmail.v1.Data;
using Google.Apis.Services;
using Google.Apis.Upload;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;
using SupportLibrary.Common.Utils.Extensions;
using SupportLibrary.Common.Utils.Helpers;
using SupportLibrary.Gmail.Options;
using SupportLibrary.Gmail.Utils;

namespace SupportLibrary.Gmail
{
    public class GmailClient : IGmailClient
    {
        private const string CONTENT_TYPE = "message/rfc822";

        private readonly CredentialsOptions _options;
        private readonly ILogger _logger;
        private GmailService _gmailService;

        public GmailClient(IOptions<CredentialsOptions> options, ILogger<GmailClient> logger = null) : 
                           this(options.Value, logger) { }        

        public GmailClient(CredentialsOptions options, ILogger<GmailClient> logger = null)
        {
            ValidateOptions(options);

            _options = options;
            _logger = logger;

            CreateServiceInstance();
        }        

        public void SendMail(string mailSubject, string mailBody, string recipients, 
                             string ccRecipients = null, string pathToAttachment = null)
        {
            ValidateFuncParams(mailSubject, mailBody, recipients);

            try
            {
                var message = new MimeMessage();

                message.From.Add(new MailboxAddress(_options.ApplicationEmail, _options.ApplicationEmail));
                message.Subject = mailSubject;
                var recipientList = recipients.Split(',').ToList().Select(x => new MailboxAddress("", x));
                message.To.AddRange(recipientList);
                var body = new TextPart(TextFormat.Plain)
                {
                    Text = mailBody
                };

                AddCcRecipientsIfExist(ccRecipients, message);
                AddAttachmentIfExist(pathToAttachment, out MimePart attachment);

                var memoryStream = new MemoryStream();

                var multipart = new Multipart("multipart")
                {
                    body,
                    attachment
                };

                message.Body = multipart;
                message.WriteTo(memoryStream);

                var gmailMessage = new Message();

                var readyMessage = _gmailService.Users.Messages.Send(gmailMessage, _options.ApplicationEmail, memoryStream, CONTENT_TYPE);
                var response = readyMessage.Upload();

                if (response.Status == UploadStatus.Failed)
                {
                    throw new Exception($"Failed to send the email. {response.Exception}");
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, $"Failed to send an email, message: {ex.Message}");

                throw;
            }
        }

        private void ValidateOptions(CredentialsOptions options)
        {
            options.ThrowExceptionIfNull();
            options.PrivateKey.ThrowExceptionIfNull();
            options.PrivateKey.ThrowExceptionIfEmptyOrWhiteSpace();
            options.ClientEmail.ThrowExceptionIfNull();
            options.ClientEmail.ThrowExceptionIfEmptyOrWhiteSpace();
            options.ApplicationEmail.ThrowExceptionIfNull();
            options.ApplicationEmail.ThrowExceptionIfEmptyOrWhiteSpace();
        }

        private void CreateServiceInstance()
        {
            try
            {
                if (!Helper.IsEmailValid(_options.ApplicationEmail))
                {
                    throw new ArgumentException("Not valid email sender was provided");
                }

                var _credential = new ServiceAccountCredential(new ServiceAccountCredential.Initializer(_options.ClientEmail)
                {
                    User = _options.ApplicationEmail,
                    Scopes = new[] { GmailService.Scope.MailGoogleCom },
                }.FromPrivateKey(_options.PrivateKey));

                _gmailService = new GmailService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = _credential,
                    ApplicationName = _options.ClientName,
                });
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, $"Failed to create instance of gmail service, message: {ex.Message}");

                throw;
            }
        }

        private void ValidateFuncParams(string mailSubject, string mailBody, string recipients)
        {
            mailSubject.ThrowExceptionIfNull();
            mailSubject.ThrowExceptionIfEmptyOrWhiteSpace();
            mailBody.ThrowExceptionIfNull();
            mailBody.ThrowExceptionIfEmptyOrWhiteSpace();
            recipients.ThrowExceptionIfNull();
            recipients.ThrowExceptionIfEmptyOrWhiteSpace();
        }

        private void AddCcRecipientsIfExist(string ccRecipients, MimeMessage message)
        {
            if (ccRecipients is null || ccRecipients.IsEmptyOrWhiteSpace()) { return; }

            message.Cc.AddRange(ccRecipients.Split(',').ToList().Select(address => new MailboxAddress("", address)));
        }

        private void AddAttachmentIfExist(string pathToFile, out MimePart attachment)
        {
            if (pathToFile is null || pathToFile.IsEmptyOrWhiteSpace() || !FileAndSystemHelper.IsFileExist(pathToFile)) 
            {
                attachment = new MimePart();
                return; 
            }

            attachment = new MimePart()
            {
                Content = new MimeContent(File.OpenRead(pathToFile), ContentEncoding.Default),
                ContentDisposition = new ContentDisposition(ContentDisposition.Attachment),
                ContentTransferEncoding = ContentEncoding.Base64,
                FileName = Path.GetFileName(pathToFile)
            };
        }        
    }
}