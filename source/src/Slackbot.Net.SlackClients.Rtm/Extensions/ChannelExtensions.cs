using Slackbot.Net.SlackClients.Rtm.Connections.Models;
using Slackbot.Net.SlackClients.Rtm.Models;

namespace Slackbot.Net.SlackClients.Rtm.Extensions
{
    internal static class ChannelExtensions
    {
        public static ChatHub ToChatHub(this Channel channel)
        {
            var newChannel = new ChatHub
            {
                Id = channel.Id,
                Name = "#" + channel.Name,
                Type = ChatHubType.Channel,
                Members = channel.Members
            };
            return newChannel;
        }
    }
}