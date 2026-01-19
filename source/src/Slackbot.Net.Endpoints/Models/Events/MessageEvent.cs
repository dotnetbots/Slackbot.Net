namespace Slackbot.Net.Endpoints.Models.Events;

public class MessageEvent : SlackEvent
{
    public string Channel { get; set; }
    public string User { get; set; }
    public string Text { get; set; }
    public string Ts { get; set; }
    public string Event_Ts { get; set; }
    public string Thread_Ts { get; set; }
    public string Channel_Type { get; set; }

    public string? Bot_id { get; set; }
    public BotProfile Bot_Profile { get; set; }
}

public class BotProfile
{
    public string Id { get; set; }
    public string Name { get; set; }
}
