using Newtonsoft.Json;
using Slackbot.Net.SlackClients.Rtm.Serialising;

namespace Slackbot.Net.SlackClients.Rtm.Connections.Sockets.Messages.Inbound
{
    //TOOD: Turn into interface?
    internal abstract class InboundMessage
    {
        [JsonProperty("type")]
        [JsonConverter(typeof(EnumConverter))]
        public MessageType MessageType { get; set; }

        public string RawData { get; set; }
    }
}
