namespace SupportLibrary.Common.Utils.Helpers
{
    public static class DateTimeHelper
    {
        private const string DEFAULT_FORMAT = "dd/MM/yyyy HH:mm:ss";        
        private static DateTime _epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public static long GetNowTimeInSecondsUnixFormat()
        {
            return ((DateTimeOffset)DateTime.Now).ToUnixTimeSeconds();
        }

        public static long GetNowTimeInMillisecondsUnixFormat()
        {
            return ((DateTimeOffset)DateTime.Now).ToUnixTimeMilliseconds();
        }

        public static string GetNowUniversalTime(string format = null)
        {
            format = format ?? DEFAULT_FORMAT;

            return DateTime.Now.ToUniversalTime().ToString(format);
        }

        public static DateTime FromUnixTimestamp(long timestamp)
        {
            return _epoch.AddSeconds(timestamp);
        }

        public static DateTime FromUnixTimestampMs(long timestampms)
        {
            return _epoch.AddMilliseconds(timestampms);
        }

        public static DateTime FromUnixTimestamp(int timestamp)
        {
            return _epoch.AddSeconds(timestamp);
        }

        public static DateTime FromUnixTimestampMs(int timestampms)
        {
            return _epoch.AddMilliseconds(timestampms);
        }

        public static int GetDayOfWeek()
        {
            return (int)DateTime.UtcNow.DayOfWeek;
        }
    }
}