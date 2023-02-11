using Newtonsoft.Json;

namespace SupportLibrary.Common.Utils.Extensions
{
    public static class JsonExtensions
    {
        public static IEnumerable<T> ReadDelimitedJson<T>(this TextReader reader, JsonSerializerSettings settings = null)
        {
            using (var jsonReader = new JsonTextReader(reader) { CloseInput = false, SupportMultipleContent = true })
            {
                var serializer = JsonSerializer.CreateDefault(settings);

                while (jsonReader.Read())
                {
                    if (jsonReader.TokenType == JsonToken.Comment) { continue; }

                    yield return serializer.Deserialize<T>(jsonReader);
                }
            }
        }              
    }
}