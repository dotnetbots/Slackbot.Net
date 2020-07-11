namespace Slackbot.Net.Endpoints.Models
{
    public class SlackEvent
    {
        public string Type { get; set; }
    }

    public class UnknownSlackEvent : SlackEvent
    {
        public string RawJson { get; set; }
    }
}