using UAParser;

namespace SupportLibrary.UserAgentParser
{
    public interface IUserAgentParserClient
    {
        Task InitializeAsync();
        void InitializeDefault();
        ClientInfo Parse(string ua);
        bool TryParse(string ua, out ClientInfo info);
    }
}