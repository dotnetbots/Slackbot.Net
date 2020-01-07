using System;
using Newtonsoft.Json;

namespace Slackbot.Net.SlackClients.Rtm.Connections.Sockets.Messages.Inbound
{
    internal class PongMessage : InboundMessage
    {
        public PongMessage()
        {
            MessageType = MessageType.Pong;
        }

        [JsonProperty("time")]
        public DateTime Timestamp { get; set; }
        [JsonProperty("reply_to")]
        public int ReplyTo { get; set; }
    }
}