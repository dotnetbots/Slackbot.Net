#nullable enable
namespace Slackbot.Net.Endpoints.Models.Events;

// https://api.slack.com/events/emoji_changed
public class EmojiChangedEvent : SlackEvent
{
    public string? Name { get; set; } // subtype: add
    public string[] Names { get; set; } = []; //subtype:  remove
    public string OldName { get; set; } // subtype: rename
    public string NewName { get; set; } // subtype: rename
    public Uri? Value { get; set; }// subtypes: add, rename
}
