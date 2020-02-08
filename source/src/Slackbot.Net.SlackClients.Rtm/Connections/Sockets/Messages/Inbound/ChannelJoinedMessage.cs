using Slackbot.Net.SlackClients.Rtm.Connections.Models;

namespace Slackbot.Net.SlackClients.Rtm.Connections.Sockets.Messages.Inbound
{
    internal class ChannelJoinedMessage : InboundMessage
    {
        public ChannelJoinedMessage()
        {
            MessageType = MessageType.Channel_Joined;
        }

        public Channel Channel { get; set; }
    }
    
}