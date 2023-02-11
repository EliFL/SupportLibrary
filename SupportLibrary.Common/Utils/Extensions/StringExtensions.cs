using Newtonsoft.Json;
using SupportLibrary.Common.Utils.Exceptions;
using SupportLibrary.Common.Utils.Helpers;
using System.Text;
using System.Xml.Serialization;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace SupportLibrary.Common.Utils.Extensions
{
    public static class StringExtensions
    {
        public static void ThrowExceptionIfEmptyOrWhiteSpace(this string value)
        {
            if (value == String.Empty)
            {
                throw new StringEmptyException($"{nameof(value)} is empty");
            }

            if (value.Trim() == String.Empty)
            {
                throw new StringWhiteSpacesOnlyException($"{nameof(value)} contains spaces only");
            }
        }

        public static bool IsEmptyOrWhiteSpace(this string value)
        {
            value.ThrowExceptionIfNull();

            return value.Trim() == String.Empty;
        }

        public static Stream ToStream(this string value, Encoding encoding = null)
        {
            StringHelper.ValidateInput(value);

            encoding = encoding ?? Encoding.Default;

            return new MemoryStream(encoding.GetBytes(value));
        }

        public static byte[] ToByteArray(this string value, Encoding encoding = null)
        {
            StringHelper.ValidateInput(value);

            encoding = encoding ?? Encoding.Default;

            return encoding.GetBytes(value);
        }

        public static string Base64Encode(this string value)
        {
            StringHelper.ValidateInput(value);

            return Convert.ToBase64String(value.ToByteArray(Encoding.UTF8));
        }

        public static string Base64Decode(this string value)
        {
            StringHelper.ValidateInput(value);

            return Convert.FromBase64String(value).ConvertToString(Encoding.UTF8);
        }

        public static T DeserializeJson<T>(this string value) where T : class
        {
            StringHelper.ValidateInput(value);

            return JsonConvert.DeserializeObject<T>(value);
        }

        public static T DeserializeXml<T>(this string value) where T : class
        {
            StringHelper.ValidateInput(value);

            var serializer = new XmlSerializer(typeof(T));

            using (StringReader stringReader = new StringReader(value))
            {
                return (T)serializer.Deserialize(stringReader);
            }
        }

        public static T DeserializeYaml<T>(this string value) where T : class
        {
            StringHelper.ValidateInput(value);

            var deserializer = new DeserializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .Build();

            return deserializer.Deserialize<T>(value);
        }        
    }
}