using Newtonsoft.Json;

namespace SupportLibrary.HttpClient
{
    public interface IHttpClientHelper
    {
        Task<HttpResponseMessage> GetAsyncWithRetry(string uri);
        Task<HttpResponseMessage> GetAsyncWithRetry(Uri uri);
        Task<HttpResponseMessage> PostAsyncWithRetry<T>(string uri, T content, JsonSerializerSettings settings = null) where T : class;
        Task<HttpResponseMessage> PostAsyncWithRetry<T>(Uri uri, T content, JsonSerializerSettings settings = null) where T : class;
        Task<HttpResponseMessage> PostAsyncWithRetry(string uri, byte[] content);
        Task<HttpResponseMessage> PostAsyncWithRetry(Uri uri, byte[] content);
        Task<HttpResponseMessage> PostAsyncWithRetry(string uri, string content);
        Task<HttpResponseMessage> PostAsyncWithRetry(Uri uri, string content);
    }
}