using System;
using Slackbot.Net.Abstractions.Handlers.Models.Rtm.MessageReceived;
using Slackbot.Net.SlackClients.Rtm.Models;
using ChatHub = Slackbot.Net.SlackClients.Rtm.Models.ChatHub;
using SlackMessage = Slackbot.Net.Abstractions.Handlers.Models.Rtm.MessageReceived.SlackMessage;
using User = Slackbot.Net.SlackClients.Rtm.Models.User;

namespace Slackbot.Net.Connections
{
    public class SlackConnectorMapper
    {
        public static SlackMessage Map(Message msg)
        {
            var slackMessage = new SlackMessage
            {
                Text = msg.Text,
                RawData = msg.RawData,
                Timestamp = msg.Timestamp,
                User = ToUser(msg.User),
                MentionsBot = msg.MentionsBot,
                ChatHub = new Abstractions.Handlers.Models.Rtm.MessageReceived.ChatHub
                {
                    Id = msg.ChatHub?.Id,
                    Name = msg.ChatHub?.Name,
                    Type = EnumToString(msg.ChatHub)
                }
            };
            return slackMessage;
        }

        private static Abstractions.Handlers.Models.Rtm.MessageReceived.User ToUser(User messageUser)
        {
            return new Abstractions.Handlers.Models.Rtm.MessageReceived.User
            {
                Id = messageUser.Id,
                Email = messageUser.Email,
                FirstName = messageUser.FirstName,
                LastName = messageUser.LastName,
                Image = messageUser.Image,
                Name = messageUser.Name,
                IsBot = messageUser.IsBot
            };

        }

        private static string EnumToString(ChatHub chatHubType)
        {
            if (chatHubType == null)
                return "Unknown";
            
            switch (chatHubType.Type)
            {
                case ChatHubType.DM:
                    return ChatHubTypes.DirectMessage;
                case ChatHubType.Channel:
                    return ChatHubTypes.Channel;
                case ChatHubType.Group:
                    return ChatHubTypes.Group;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}