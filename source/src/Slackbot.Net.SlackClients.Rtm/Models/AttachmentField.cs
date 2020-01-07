using Newtonsoft.Json;

namespace Slackbot.Net.SlackClients.Rtm.Models
{
    public class AttachmentField
    {
        [JsonProperty(PropertyName = "short")]
        public bool IsShort { get; set; }

        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }

        [JsonProperty(PropertyName = "value")]
        public string Value { get; set; }
    }
}