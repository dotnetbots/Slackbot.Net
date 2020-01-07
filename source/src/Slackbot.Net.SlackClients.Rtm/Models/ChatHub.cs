namespace Slackbot.Net.SlackClients.Rtm.Models
{
    /// <summary>
    /// This represents a place in Slack where people can chat - typically a channel, group, or DM.
    /// </summary>
    public class ChatHub
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public ChatHubType Type { get; set; }
        public string[] Members { get; set; }
    }
}