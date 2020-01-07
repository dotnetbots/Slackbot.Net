using Slackbot.Net.SlackClients.Rtm.Connections.Sockets.Messages.Inbound;
using Slackbot.Net.SlackClients.Rtm.Models;
using MessageSubType = Slackbot.Net.SlackClients.Rtm.Models.MessageSubType;

namespace Slackbot.Net.SlackClients.Rtm.Extensions
{
    internal static class MessageSubTypeExtensions
    {
        public static MessageSubType ToSlackMessageSubType(this Connections.Sockets.Messages.Inbound.MessageSubType subType)
        {
            return (MessageSubType)subType;
        }
    }
}