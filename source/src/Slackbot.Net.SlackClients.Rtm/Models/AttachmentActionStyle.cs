using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Slackbot.Net.SlackClients.Rtm.Models
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum AttachmentActionStyle
    {
        [EnumMember(Value = "default")]
        Default,
        [EnumMember(Value = "primary")]
        Primary,
        [EnumMember(Value = "danger")]
        Danger
    }
}