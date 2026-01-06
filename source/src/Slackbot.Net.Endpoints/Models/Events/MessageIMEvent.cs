namespace Slackbot.Net.Endpoints.Models.Events;

public class MessageIMEvent : SlackEvent
{
    public string Channel { get; set; }
    public string User { get; set; }
    public string Text { get; set; }
    public string Ts { get; set; }
    public string Event_Ts { get; set; }
    public string Channel_Type { get; set; }
}
