namespace SupportLibrary.Common.Utils.Extensions
{
    public static class DateTimeExtensions
    {
        private static DateTime _epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public static long ToUnixTimestamp(this DateTime datetime)
        {
            return (long)datetime.Subtract(_epoch).TotalSeconds;
        }

        public static long ToUnixTimestampMs(this DateTime datetime)
        {
            return (long)datetime.Subtract(_epoch).TotalMilliseconds;
        }
    }
}