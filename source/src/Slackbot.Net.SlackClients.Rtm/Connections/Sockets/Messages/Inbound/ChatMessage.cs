﻿using Newtonsoft.Json;
using Slackbot.Net.SlackClients.Rtm.Serialising;

namespace Slackbot.Net.SlackClients.Rtm.Connections.Sockets.Messages.Inbound
{
    internal class ChatMessage : InboundMessage
    {
        public ChatMessage()
        {
            MessageType = MessageType.Message;
        }

        [JsonProperty("subtype")]
        [JsonConverter(typeof(EnumConverter))]
        public MessageSubType MessageSubType { get; set; }

        public string Channel { get; set; }
        public string User { get; set; }
        public string Text { get; set; }
        public string Team { get; set; }
        public File[] Files { get; set; }

        [JsonProperty("ts")]
        public double Timestamp { get; set; }

    }
}
