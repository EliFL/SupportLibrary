namespace SupportLibrary.UserAgentParser.Options
{
    public class UserAgentParserOptions
    {
        public UserAgentParserOptions() { }

        public UserAgentParserOptions(string url)
        {
            Url = url;
        }

        public static UserAgentParserOptions Default { get; private set; } = new UserAgentParserOptions(DefaultResources.DEFAULT_YAML);

        public string Url { get; set; }
    }
}