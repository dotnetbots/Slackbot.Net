using Newtonsoft.Json;

namespace Slackbot.Net.SlackClients.Rtm.Connections.Sockets.Messages.Outbound
{
    internal class TypingIndicatorMessage : BaseMessage
    {
        public TypingIndicatorMessage()
        {
            Type = "typing";
        }

        [JsonProperty("channel")]
        public string Channel { get; set; } 
    }
}