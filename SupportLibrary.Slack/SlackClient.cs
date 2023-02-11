using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SupportLibrary.Slack.Models;
using SupportLibrary.Slack.Options;
using SupportLibrary.Common.Utils.Extensions;
using System.Text;

namespace SupportLibrary.Slack
{
    public class SlackClient : ISlackClient
    {
        private readonly SlackOptions _options;
        private readonly ILogger _logger;
        private readonly HttpClient _client;
        private readonly JsonSerializerSettings _serializerSettings;

        public SlackClient(IOptions<SlackOptions> options, ILogger<SlackClient> logger = null) : 
            this(options.Value, logger) { }

        public SlackClient(SlackOptions options, ILogger<SlackClient> logger = null)
        {
            ValidateOptions(options);

            _options = options;
            _logger = logger;
            _client = new HttpClient();
            _serializerSettings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }; //TODO implement an option to add it to constructor
        }        

        public SlackPayload PreparePayload(SlackMessageType messageType, string title, string text = null)
        {
            messageType.ThrowExceptionIfNull();
            title.ThrowExceptionIfNull();

            var payload = new SlackPayload
            {
                Attachments = new List<Attachment>()
            };

            payload.Attachments.Add(new Attachment
            {
                Color = messageType.Value,
                Title = title,
                Text = text
            });

            return payload;
        }

        public async Task SendMessageAsync(SlackPayload payload)
        {
            payload.ThrowExceptionIfNull();

            try
            {
                var response = await InternalPostAsync(payload);

                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, $"Failed to send message to {_options.ReportChannel}, message: {ex.Message}");

                throw;
            }            
        }

        public async Task<bool> TrySendMessageAsync(SlackPayload payload)
        {
            payload.ThrowExceptionIfNull();

            try 
            {
                var response = await InternalPostAsync(payload);
                if(!response.IsSuccessStatusCode)
                {
                    _logger?.LogError($"Failed to send message to {_options.ReportChannel}, Response status code: {response.StatusCode}");

                    return false;
                }

                return true;
            }
            catch(Exception ex)
            {
                _logger?.LogError(ex, $"Failed to send message to {_options.ReportChannel}, message: {ex.Message}");

                return false;
            }
        }

        private void ValidateOptions(SlackOptions options)
        {
            options.ThrowExceptionIfNull();
            options.ReportChannel.ThrowExceptionIfNull();
            options.ReportChannel.ThrowExceptionIfEmptyOrWhiteSpace();
        }

        private async Task<HttpResponseMessage> InternalPostAsync(SlackPayload payload)
        {
            var dataToSend = InternalPreparePayload(payload);

            return await _client.PostAsync(_options.ReportChannel, dataToSend);
        }

        private StringContent InternalPreparePayload(SlackPayload payload)
        {
            var payloadJson = JsonConvert.SerializeObject(payload, _serializerSettings);
            var preparedPayload = new StringContent(payloadJson, Encoding.UTF8, "application/json");

            return preparedPayload;
        }
    }
}