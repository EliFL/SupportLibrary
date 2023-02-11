using Polly.Retry;
using Polly;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using SupportLibrary.HttpClient.Options;
using SupportLibrary.Common.Utils.Extensions;

namespace SupportLibrary.HttpClient
{
    public class HttpClientHelper : IHttpClientHelper
    {
        private readonly RetryOptions _options;
        private readonly ILogger _logger;
        private AsyncRetryPolicy<HttpResponseMessage> _retryPolicy;

        public HttpClientHelper(ILogger<HttpClientHelper> logger = null) :
            this(RetryOptions.Default, logger) { }

        public HttpClientHelper(IOptions<RetryOptions> options, ILogger<HttpClientHelper> logger = null) :
            this(options.Value, logger) { }

        public HttpClientHelper(RetryOptions options, ILogger<HttpClientHelper> logger = null)
        {
            ValidateOptions(options);

            _options = options;
            _logger = logger;

            _retryPolicy = Policy.HandleResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode)
                                 .Or<HttpRequestException>()
                                 .Or<TaskCanceledException>()
                                 .WaitAndRetryAsync(_options.RetryCount, sleepDuration => _options.SleepDurationMilliseconds);
        }        

        public async Task<HttpResponseMessage> GetAsyncWithRetry(string uri)
        {
            return await GetAsyncWithRetry(CreateUri(uri));
        }

        public async Task<HttpResponseMessage> GetAsyncWithRetry(Uri uri)
        {
            uri.ThrowExceptionIfNull();

            try
            {
                var response = await SendAsyncWithRetry(client => client.GetAsync(uri));
                response.EnsureSuccessStatusCode();

                return response;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, $"Failed to send GET request to {uri.AbsoluteUri}, message: {ex.Message}");

                throw;
            }
        }

        public async Task<HttpResponseMessage> PostAsyncWithRetry<T>(string uri, T content,
                                                                     JsonSerializerSettings settings = null) where T : class
        {
            return await PostAsyncWithRetry(CreateUri(uri), content, settings);
        }

        public async Task<HttpResponseMessage> PostAsyncWithRetry<T>(Uri uri, T content,
                                                                     JsonSerializerSettings settings = null) where T : class
        {
            return await PostAsyncWithRetry(uri, JsonConvert.SerializeObject(content, settings));
        }

        public async Task<HttpResponseMessage> PostAsyncWithRetry(string uri, byte[] content)
        {
            return await PostAsyncWithRetry(CreateUri(uri), content);
        }

        public async Task<HttpResponseMessage> PostAsyncWithRetry(Uri uri, byte[] content)
        {
            uri.ThrowExceptionIfNull();

            if (content is null || content.Length <= 0)
            {
                throw new ArgumentException("Provided content cannot be null or empty");
            }

            try
            {
                var response = await SendAsyncWithRetry(client =>
                {
                    var request = new HttpRequestMessage(HttpMethod.Post, uri);
                    request.Content = new ByteArrayContent(content);
                    request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                    return client.SendAsync(request);
                });
                response.EnsureSuccessStatusCode();

                return response;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, $"Failed to send POST request to {uri.AbsoluteUri}, message: {ex.Message}");

                throw;
            }
        }

        public async Task<HttpResponseMessage> PostAsyncWithRetry(string uri, string content)
        {
            return await PostAsyncWithRetry(CreateUri(uri), content);
        }

        public async Task<HttpResponseMessage> PostAsyncWithRetry(Uri uri, string content)
        {
            uri.ThrowExceptionIfNull();
            content.ThrowExceptionIfNull();
            content.ThrowExceptionIfEmptyOrWhiteSpace();

            try
            {
                var response = await SendAsyncWithRetry(client =>
                {
                    var request = new HttpRequestMessage(HttpMethod.Post, uri);
                    request.Content = new StringContent(content, Encoding.UTF8, "application/json");
                    return client.SendAsync(request);
                });
                response.EnsureSuccessStatusCode();

                return response;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, $"Failed to send POST request to {uri.AbsoluteUri}, message: {ex.Message}");

                throw;
            }
        }

        private void ValidateOptions(RetryOptions options)
        {
            options.ThrowExceptionIfNull();

            if (options.RetryCount <= 0)
            {
                throw new ArgumentException($"Retry count should be possitive int, wrong value: {options.RetryCount}");
            }
        }

        private async Task<HttpResponseMessage> SendAsyncWithRetry(Func<System.Net.Http.HttpClient, Task<HttpResponseMessage>> requestFactory)
        {
            using (var client = new System.Net.Http.HttpClient())
            {
                return await _retryPolicy.ExecuteAsync(() => requestFactory(client));
            }
        }

        private Uri CreateUri(string uri)
        {
            uri.ThrowExceptionIfNull();
            uri.ThrowExceptionIfEmptyOrWhiteSpace();

            return new Uri(uri, UriKind.RelativeOrAbsolute);
        }
    }
}
