using Slackbot.Net.SlackClients.Rtm.Connections.Models;
using Slackbot.Net.SlackClients.Rtm.Models;

namespace Slackbot.Net.SlackClients.Rtm.Extensions
{
    internal static class GroupExtensions
    {
        public static ChatHub ToChatHub(this Group group)
        {
            var newGroup = new ChatHub
            {
                Id = group.Id,
                Name = "#" + group.Name,
                Type = ChatHubType.Group,
                Members = group.Members
            };

            return newGroup;
        }
    }
}