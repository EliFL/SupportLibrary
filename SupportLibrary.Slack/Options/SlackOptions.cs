namespace SupportLibrary.Slack.Options
{
    public class SlackOptions
    {
        public SlackOptions() { }

        public SlackOptions(string reportChannel)
        {
            ReportChannel = reportChannel;
        }

        public string ReportChannel { get; set; }
    }
}