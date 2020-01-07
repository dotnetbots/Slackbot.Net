using System.Collections.Generic;

namespace Slackbot.Net.SlackClients.Rtm.Models
{
    internal class ConnectionInformation
    {
        public string SlackKey { get; set; }
        public ContactDetails Self { get; set; } = new ContactDetails();
        public ContactDetails Team { get; set; } = new ContactDetails();
        public Dictionary<string, User> Users { get; set; } = new Dictionary<string, User>();
        public Dictionary<string, ChatHub> SlackChatHubs { get; set; } = new Dictionary<string, ChatHub>();
    }
}