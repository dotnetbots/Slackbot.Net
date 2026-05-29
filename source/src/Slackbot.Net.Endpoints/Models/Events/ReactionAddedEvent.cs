namespace Slackbot.Net.Endpoints.Models.Events;

// https://docs.slack.dev/reference/events/reaction_added
public class ReactionAddedEvent : SlackEvent
{
    public string User { get; set; }
    public string Reaction { get; set; }
    public string Item_User { get; set; }
    public string Event_Ts { get; set; }
    public ReactionItem Item { get; set; }
}

public class ReactionItem
{
    public string Type { get; set; }          // message | file | file_comment
    public string Channel { get; set; }        // message
    public string Ts { get; set; }             // message
    public string File { get; set; }           // file, file_comment
    public string File_Comment { get; set; }   // file_comment
}
