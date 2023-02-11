using System.Text;

namespace SupportLibrary.Common.Utils.Extensions
{
    public static class ByteArrayExtensions
    {
        public static string ConvertToString(this byte[] bytearray, Encoding encoding = null)
        {
            bytearray.ThrowExceptionIfNull();

            if (bytearray.Length == 0) { return String.Empty; }

            encoding = encoding ?? Encoding.Default;

            return encoding.GetString(bytearray);
        }
    }
}