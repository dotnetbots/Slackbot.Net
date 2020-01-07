using System.Collections.Generic;

namespace Slackbot.Net.SlackClients.Rtm.Models
{
    public class Message
    {
        public ChatHub ChatHub { get; set; }
        public bool MentionsBot { get; set; }
        public string RawData { get; set; }
        public string Text { get; set; }
        public User User { get; set; }
        public double Timestamp { get; set; }
        public MessageSubType MessageSubType { get; set; }
        public IEnumerable<File> Files { get; set; }
    }
}