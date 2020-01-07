namespace Slackbot.Net.Abstractions.Handlers.Models.Rtm.MessageReceived
{
    public class SlackMessage
    {
        public ChatHub ChatHub { get; set; }

        public string Text { get; set; }
        
        public bool MentionsBot { get; set; }
        public User User { get; set; }

        public double Timestamp { get; set; }
        public string RawData { get; set; }
    }

    public class ChatHub
    {
        public string Id { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
    }

    public class ChatHubTypes
    {
        public const string DirectMessage = "DM";
        public const string Channel = "Channel";
        public const string Group = "Group";
        public const string Unknown = "Unknown";
    }

    public class User
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Image { get; set; }
        public bool IsBot { get; set; }

    }
}