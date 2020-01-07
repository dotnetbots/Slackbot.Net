using Newtonsoft.Json;

namespace Slackbot.Net.SlackClients.Rtm.Connections.Sockets.Messages.Outbound
{
    internal abstract class BaseMessage
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }
    }
}