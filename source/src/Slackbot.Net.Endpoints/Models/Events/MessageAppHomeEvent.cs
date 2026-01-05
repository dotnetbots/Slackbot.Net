namespace Slackbot.Net.Endpoints.Models.Events;

public class MessageAppHomeEvent : SlackEvent
{
    public string Text { get; set; }
    public string User { get; set; }
    public string Channel { get; set; }
    public string Ts { get; set; }
    public string Event_Ts { get; set; }
    public string Thread_Ts { get; set; }
}
