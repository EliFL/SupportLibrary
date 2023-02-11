namespace SupportLibrary.Slack.Options
{
    public class SlackMessageType
    {      
        private SlackMessageType(string value)
        {
            Value = value;
        }

        public static SlackMessageType Success => new SlackMessageType("#008000");
        public static SlackMessageType Warning => new SlackMessageType("#FFC300");
        public static SlackMessageType Info => new SlackMessageType("#009dff");
        public static SlackMessageType Error => new SlackMessageType("#DC143C");

        public string Value { get; private set; }
    }
}