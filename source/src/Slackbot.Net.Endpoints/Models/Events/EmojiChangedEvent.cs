#nullable enable
namespace Slackbot.Net.Endpoints.Models.Events;

// https://api.slack.com/events/emoji_changed
public class EmojiChangedEvent : SlackEvent
{
    public const string SubTypeAdd = "add";
    public const string SubTypeRemove = "remove";
    public const string SubTypeRename = "rename";

    public string SubType { get; init; } = string.Empty; // subtype: add, remove, rename
    public string? Name { get; set; } // subtype: add
    public string[] Names { get; set; } = []; //subtype:  remove
    public string OldName { get; set; } // subtype: rename
    public string NewName { get; set; } // subtype: rename
    public Uri? Value { get; set; }// subtypes: add, rename

    public EmojiChange CreateSubType() => SubType switch
    {
        SubTypeAdd => new EmojiAdded { Name = Name!, Value = Value! },
        SubTypeRemove => new EmojiRemoved { Names = Names },
        SubTypeRename => new EmojiRenamed { OldName = OldName, NewName = NewName, Value = Value },
        _ => new UnknownEmojiChange()
    };
}

public class EmojiChange();

public class EmojiAdded : EmojiChange
{
    public required string Name { get; init; }
    public Uri Value { get; init; }
}

public class EmojiRemoved : EmojiChange
{
    public string[] Names { get; set; } = [];
}

public class EmojiRenamed : EmojiChange
{
    public string OldName { get; set; } // subtype: rename
    public string NewName { get; set; } // subtype: rename
    public Uri? Value { get; set; }// subtypes: add, rename
}

public class UnknownEmojiChange() : EmojiChange;

