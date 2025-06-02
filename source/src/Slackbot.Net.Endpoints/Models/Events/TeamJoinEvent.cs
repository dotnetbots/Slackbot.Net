namespace Slackbot.Net.Endpoints.Models.Events;

public class TeamJoinEvent : SlackEvent
{
    public string User { get; set; }
}
