using Newtonsoft.Json;

namespace SupportLibrary.Slack.Models
{
    public class SlackPayload
    {
        [JsonProperty(PropertyName = "attachments")]
        public List<Attachment> Attachments { get; set; }
    }

    public class Attachment
    {
        [JsonProperty(PropertyName = "color")]
        public string Color { get; set; }

        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }        

        [JsonProperty(PropertyName = "text")]
        public string Text { get; set; }
    }
}