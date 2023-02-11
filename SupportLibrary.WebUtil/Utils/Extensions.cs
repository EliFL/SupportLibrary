using Microsoft.AspNetCore.Http;
using SupportLibrary.Common.Utils.Extensions;
using System.Web;

namespace SupportLibrary.WebUtil.Utils
{
    public static class Extensions
    {
        private const string REFERRER_EXCEPTION_MESSAGE = "Failed to get referrer from request";

        public static string GetIp(this HttpRequest request)
        {
            request.ThrowExceptionIfNull();

            var remoteIpAddress = String.Empty;

            if (request.Headers.ContainsKey("X-Forwarded-For"))
            {
                remoteIpAddress = request.Headers["X-Forwarded-For"].ToString()?.Split(',')?.FirstOrDefault();
            }

            return !String.IsNullOrEmpty(remoteIpAddress) ?
                   remoteIpAddress : !String.IsNullOrEmpty(request.HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString()) ?
                   request.HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString() : throw new Exception("Failed to get IP address from request");
        }

        public static string GetDomain(this HttpRequest request)
        {
            request.ThrowExceptionIfNull();

            var host = String.Empty;

            if(request.Headers.ContainsKey("X-Forwarded-Host"))
            {
                host = request.Headers["X-Forwarded-Host"].FirstOrDefault();
            }

            return !String.IsNullOrEmpty(host) ? host : !String.IsNullOrEmpty(request.Host.Value) ? 
                   request.Host.Value : throw new Exception("Failed to get host from request");
        }

        public static string GetUserAgent(this HttpRequest request)
        {
            request.ThrowExceptionIfNull();

            var userAgent = request?.Headers["User-Agent"] ?? String.Empty;

            if(userAgent.ToString().IsEmptyOrWhiteSpace())
            {
                throw new Exception("Failed to get UserAgent from request");
            }

            return userAgent;
        }

        public static string GetReferrer(this HttpRequest request)
        {
            request.ThrowExceptionIfNull();

            if(request.Headers.TryGetValue("Referer", out var referrer))
            {
                return !referrer.ToString().IsEmptyOrWhiteSpace() ? referrer : throw new Exception(REFERRER_EXCEPTION_MESSAGE); 
            }
            else
            {
                throw new Exception(REFERRER_EXCEPTION_MESSAGE);
            }
        }        

        public static string GetQueryString(this HttpRequest request)
        {
            request.ThrowExceptionIfNull();

            return request.QueryString.HasValue ? String.Concat(request.QueryString.ToString().Skip(1)) : String.Empty;
        }

        public static Dictionary<string, string> GetQueryParametersDictionary(this Uri url)
        {
            url.ThrowExceptionIfNull();

            var outputQueryParameters = HttpUtility.ParseQueryString(url.Query);
            return outputQueryParameters.AllKeys.Where(key => key is not null)
                                                .ToDictionary(key => key!, key => outputQueryParameters[key] ?? String.Empty);
        }
    }
}