using UAParser;

namespace SupportLibrary.UserAgentParser.Utils
{
    public static class Extensions
    {
        public static bool IsWindows(this ClientInfo info)
        {
            return info?.OS.Family.ToLower() == "windows";
        }

        public static bool? IsMac(this ClientInfo info)
        {
            return info?.OS.Family.ToLower().Contains("mac");
        }

        public static bool IsLinus(this ClientInfo info)
        {
            return info?.OS.Family.ToLower() == "linux";
        }

        public static bool IsAndroid(this ClientInfo info)
        {
            return info?.OS.Family.ToLower() == "android";
        }

        public static bool IsIOS(this ClientInfo info)
        {
            return info?.OS.Family.ToLower() == "ios";
        }

        public static bool IsChrome(this ClientInfo info)
        {
            return info?.UA.Family.ToLower() == "chrome";
        }

        public static bool IsEdge(this ClientInfo info)
        {
            return info?.UA.Family.ToLower() == "edge";
        }

        public static bool IsFirefox(this ClientInfo info)
        {
            return info?.UA.Family.ToLower() == "firefox";
        }

        public static bool IsSafari(this ClientInfo info)
        {
            return info?.UA.Family.ToLower() == "safari";
        }
    }
}