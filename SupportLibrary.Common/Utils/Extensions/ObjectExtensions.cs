using Newtonsoft.Json;
using System.Xml;
using System.Xml.Serialization;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace SupportLibrary.Common.Utils.Extensions
{
    public static class ObjectExtensions
    {
        public static void ThrowExceptionIfNull<T>(this T @object) where T : class
        {
            if (@object is null)
            {
                throw new ArgumentNullException($"{typeof(T).Name} {nameof(@object)}");
            }
        }

        public static string Serialize<T>(this T @object, Newtonsoft.Json.Formatting formatting = Newtonsoft.Json.Formatting.Indented, 
                                          JsonSerializerSettings settings = null) where T : class
        {
            @object.ThrowExceptionIfNull();

            return JsonConvert.SerializeObject(@object, formatting, settings);
        }

        public static string Serialize<T>(this T @object, System.Xml.Formatting formatting = System.Xml.Formatting.Indented)
            where T : class
        {
            @object.ThrowExceptionIfNull();

            var serializer = new XmlSerializer(typeof(T));

            using (StringWriter stringWriter = new StringWriter())
            {
                using (XmlTextWriter xmlWriter = new XmlTextWriter(stringWriter)
                {
                    Formatting = formatting
                })
                {
                    serializer.Serialize(xmlWriter, @object);
                    return stringWriter.ToString();
                }
            }
        }

        public static string Serialize<T>(this T @object, DefaultValuesHandling valuesHandling = DefaultValuesHandling.OmitNull)
            where T : class
        {
            @object.ThrowExceptionIfNull();

            var builder = new SerializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .ConfigureDefaultValuesHandling(valuesHandling);

            var serializer = builder.Build();
            return serializer.Serialize(@object);
        }
    }
}