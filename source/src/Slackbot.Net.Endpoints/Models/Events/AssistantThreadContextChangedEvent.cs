namespace Slackbot.Net.Endpoints.Models.Events;

public class AssistantThreadContextChangedEvent : SlackEvent
{
    public AssistantThread Assistant_Thread { get; set; }
    public string Event_Ts { get; set; }
}
