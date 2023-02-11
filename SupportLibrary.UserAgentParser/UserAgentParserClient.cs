using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SupportLibrary.Common.Utils.Extensions;
using SupportLibrary.HttpClient;
using SupportLibrary.UserAgentParser.Options;
using UAParser;

namespace SupportLibrary.UserAgentParser
{
    public class UserAgentParserClient : IUserAgentParserClient
    {
        private readonly UserAgentParserOptions _options;
        private readonly ILogger _logger;
        private Parser _parser;
        private readonly HttpClientHelper _client;
        private bool _isInit;

        public UserAgentParserClient(ILogger<UserAgentParserClient> logger = null) 
            : this(UserAgentParserOptions.Default, logger) { }

        public UserAgentParserClient(IOptions<UserAgentParserOptions> options, ILogger<UserAgentParserClient> logger = null) 
            : this (options.Value, logger) { }

        public UserAgentParserClient(UserAgentParserOptions options, ILogger<UserAgentParserClient> logger = null)
        {
            ValidateOptions(options);

            _options = options;
            _logger = logger;    
            _client = new HttpClientHelper();
            _isInit = false;
        }        
        
        public async Task InitializeAsync()
        {
            if(_isInit)
            {
                _logger?.LogInformation("The instanse was already initialized, going to reinitialize it");
            }

            _parser = Parser.FromYaml(await _client.GetAsyncWithRetry(_options.Url).Result.Content.ReadAsStringAsync());

            _isInit = true;
        }

        public void InitializeDefault()
        {
            if(_isInit)
            {
                _logger?.LogInformation("The instanse was already initialized, going to reinitialize it");
            }

            _parser = Parser.GetDefault();

            _isInit = true;
        }

        public ClientInfo Parse(string ua)
        {
            if(!_isInit)
            {
                throw new InvalidOperationException("UaParserManager is not initialized, you have to call Initialize/InitializeDefault before using");
            }

            if(_parser is null)
            {
                throw new Exception("Parser cannot be null");
            }

            try
            {
                return _parser.Parse(ua);
            }
            catch(Exception ex)
            {
                _logger?.LogError(ex, $"Failed to parse User-Agent, value: {ua}, message: {ex.Message}");

                throw;
            }            
        }

        public bool TryParse(string ua, out ClientInfo info)
        {
            info = null;

            if (!_isInit || _parser is null)
            {
                return false;
            }

            try
            {
                info = Parse(ua);
                return true;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, $"Failed to parse User-Agent, value: {ua}, message: {ex.Message}");
            }

            return false;
        }

        private void ValidateOptions(UserAgentParserOptions options)
        {
            options.ThrowExceptionIfNull();
            options.Url.ThrowExceptionIfNull();
            options.Url.ThrowExceptionIfEmptyOrWhiteSpace();
        }
    }
}