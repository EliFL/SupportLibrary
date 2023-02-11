using SupportLibrary.Slack.Models;
using SupportLibrary.Slack.Options;

namespace SupportLibrary.Slack
{
    public interface ISlackClient
    {
        SlackPayload PreparePayload(SlackMessageType messageType, string title, string text = null);
        Task SendMessageAsync(SlackPayload payload);
        Task<bool> TrySendMessageAsync(SlackPayload payload);
    }
}