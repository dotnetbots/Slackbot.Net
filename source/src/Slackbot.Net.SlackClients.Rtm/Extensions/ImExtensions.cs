using System.Linq;
using Slackbot.Net.SlackClients.Rtm.Connections.Models;
using Slackbot.Net.SlackClients.Rtm.Models;
using User = Slackbot.Net.SlackClients.Rtm.Models.User;

namespace Slackbot.Net.SlackClients.Rtm.Extensions
{
    internal static class ImExtensions
    {
        public static ChatHub ToChatHub(this Im im, User[] users)
        {
            User user = users.FirstOrDefault(x => x.Id == im.User);
            return new ChatHub
            {
                Id = im.Id,
                Name = "@" + (user == null ? im.User : user.Name),
                Type = ChatHubType.DM
            };
        }
    }
}
