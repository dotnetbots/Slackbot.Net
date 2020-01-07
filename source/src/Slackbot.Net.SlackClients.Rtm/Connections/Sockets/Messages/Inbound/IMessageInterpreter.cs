namespace Slackbot.Net.SlackClients.Rtm.Connections.Sockets.Messages.Inbound
{
    internal interface IMessageInterpreter
    {
        InboundMessage InterpretMessage(string json);
    }
}