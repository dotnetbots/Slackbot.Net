using System.Collections.Generic;
using System.Linq;
using Slackbot.Net.SlackClients.Rtm.Connections.Models;
using Slackbot.Net.SlackClients.Rtm.Connections.Responses;
using Slackbot.Net.SlackClients.Rtm.Extensions;
using Slackbot.Net.SlackClients.Rtm.Models;
using User = Slackbot.Net.SlackClients.Rtm.Models.User;

namespace Slackbot.Net.SlackClients.Rtm
{
    internal static class ConnectionInformationMapper
    {
        internal static ConnectionInformation CreateConnectionInformation(string slackKey, HandshakeResponse handshakeResponse)
        {
            var users = GenerateUsers(handshakeResponse.Users);
            var slackChatHubs = GetChatHubs(handshakeResponse, users.Values.ToArray());
            var connectionInfo = new ConnectionInformation
            {
                SlackKey = slackKey,
                Self = new ContactDetails {Id = handshakeResponse.Self.Id, Name = handshakeResponse.Self.Name},
                Team = new ContactDetails {Id = handshakeResponse.Team.Id, Name = handshakeResponse.Team.Name},
                Users = users,
                SlackChatHubs = slackChatHubs,
            };
            return connectionInfo;
        }

        private static Dictionary<string, User> GenerateUsers(Connections.Models.User[] users)
        {
            return users.ToDictionary(user => user.Id, user => user.ToSlackUser());
        }

        private static Dictionary<string, ChatHub> GetChatHubs(HandshakeResponse handshakeResponse, User[] users)
        {
            var hubs = new Dictionary<string, ChatHub>();

            foreach (Channel channel in handshakeResponse.Channels.Where(x => !x.IsArchived))
            {
                if (channel.IsMember)
                {
                    var newChannel = channel.ToChatHub();
                    hubs.Add(channel.Id, newChannel);
                }
            }

            foreach (Group group in handshakeResponse.Groups.Where(x => !x.IsArchived))
            {
                if ((group.Members ?? new string[0]).Any(x => x == handshakeResponse.Self.Id))
                {
                    var newGroup = group.ToChatHub();
                    hubs.Add(group.Id, newGroup);
                }
            }

            foreach (Im im in handshakeResponse.Ims)
            {
                hubs.Add(im.Id, im.ToChatHub(users));
            }

            return hubs;
        }
    }
}