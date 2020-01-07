namespace Slackbot.Net.SlackClients.Rtm.Connections.Sockets.Messages.Inbound
{
    internal class UnknownMessage : InboundMessage
    {
        public UnknownMessage()
        {
            MessageType = MessageType.Unknown;
        }
    }
}