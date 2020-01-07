using System;

namespace Slackbot.Net.SlackClients.Rtm.Connections.Sockets.Messages.Outbound
{
    internal class PingMessage : BaseMessage
    {
        public DateTime Timestamp { get; } = DateTime.Now;

        public PingMessage()
        {
            Type = "ping";
        }
    }
}
