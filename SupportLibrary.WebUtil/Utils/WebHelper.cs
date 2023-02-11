using SupportLibrary.Common.Utils.Extensions;
using System.Text.RegularExpressions;
using System.Web;

namespace SupportLibrary.WebUtil.Utils
{
    public static class WebHelper
    {
        private const string IP_V4_REGEX = "^(?:(?:^|\\.)(?:2(?:5[0-5]|[0-4]\\d)|1?\\d?\\d)){4}$";
        private const string IP_V4_OR_V6_REGEX = "((^\\s*((([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])\\.){3}([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5]))\\s*$)|(^\\s*((([0-9A-Fa-f]{1,4}:){7}([0-9A-Fa-f]{1,4}|:))|(([0-9A-Fa-f]{1,4}:){6}(:[0-9A-Fa-f]{1,4}|((25[0-5]|2[0-4]\\d|1\\d\\d|[1-9]?\\d)(\\.(25[0-5]|2[0-4]\\d|1\\d\\d|[1-9]?\\d)){3})|:))|(([0-9A-Fa-f]{1,4}:){5}(((:[0-9A-Fa-f]{1,4}){1,2})|:((25[0-5]|2[0-4]\\d|1\\d\\d|[1-9]?\\d)(\\.(25[0-5]|2[0-4]\\d|1\\d\\d|[1-9]?\\d)){3})|:))|(([0-9A-Fa-f]{1,4}:){4}(((:[0-9A-Fa-f]{1,4}){1,3})|((:[0-9A-Fa-f]{1,4})?:((25[0-5]|2[0-4]\\d|1\\d\\d|[1-9]?\\d)(\\.(25[0-5]|2[0-4]\\d|1\\d\\d|[1-9]?\\d)){3}))|:))|(([0-9A-Fa-f]{1,4}:){3}(((:[0-9A-Fa-f]{1,4}){1,4})|((:[0-9A-Fa-f]{1,4}){0,2}:((25[0-5]|2[0-4]\\d|1\\d\\d|[1-9]?\\d)(\\.(25[0-5]|2[0-4]\\d|1\\d\\d|[1-9]?\\d)){3}))|:))|(([0-9A-Fa-f]{1,4}:){2}(((:[0-9A-Fa-f]{1,4}){1,5})|((:[0-9A-Fa-f]{1,4}){0,3}:((25[0-5]|2[0-4]\\d|1\\d\\d|[1-9]?\\d)(\\.(25[0-5]|2[0-4]\\d|1\\d\\d|[1-9]?\\d)){3}))|:))|(([0-9A-Fa-f]{1,4}:){1}(((:[0-9A-Fa-f]{1,4}){1,6})|((:[0-9A-Fa-f]{1,4}){0,4}:((25[0-5]|2[0-4]\\d|1\\d\\d|[1-9]?\\d)(\\.(25[0-5]|2[0-4]\\d|1\\d\\d|[1-9]?\\d)){3}))|:))|(:(((:[0-9A-Fa-f]{1,4}){1,7})|((:[0-9A-Fa-f]{1,4}){0,5}:((25[0-5]|2[0-4]\\d|1\\d\\d|[1-9]?\\d)(\\.(25[0-5]|2[0-4]\\d|1\\d\\d|[1-9]?\\d)){3}))|:)))(%.+)?\\s*$))";

        public static string EncodeUrl(string value)
        {
            value.ThrowExceptionIfNull();
            if(value.IsEmptyOrWhiteSpace()) { return String.Empty; }

            return HttpUtility.UrlEncode(value);
        }

        public static string DecodeUrl(string value)
        {
            value.ThrowExceptionIfNull();
            if (value.IsEmptyOrWhiteSpace()) { return String.Empty; }

            return HttpUtility.UrlDecode(value);
        }

        public static bool IsValidIPV4(string ip, RegexOptions regexOptions = RegexOptions.IgnoreCase)
        {
            return ResolveIsMatch(ip, regexOptions, (value, options) => Regex.IsMatch(value, IP_V4_REGEX, options));
        }

        public static bool IsValidIPV6(string ip, RegexOptions regexOptions = RegexOptions.IgnoreCase)
        {
            return ResolveIsMatch(ip, regexOptions, (value, options) => 
                                     !Regex.IsMatch(value, IP_V4_REGEX, options) 
                                     && Regex.IsMatch(value, IP_V4_OR_V6_REGEX, options));
        }

        public static bool IsValidIPV4OrIPV6(string ip, RegexOptions regexOptions = RegexOptions.IgnoreCase)
        {
            return ResolveIsMatch(ip, regexOptions, (value, options) => Regex.IsMatch(value, IP_V4_OR_V6_REGEX, options));
        }


        private static bool ResolveIsMatch(string ip, RegexOptions regexOptions, Func<string, RegexOptions, bool> func)
        {
            ip.ThrowExceptionIfNull();
            ip.ThrowExceptionIfEmptyOrWhiteSpace();

            return func(ip, regexOptions);
        }
    }
}