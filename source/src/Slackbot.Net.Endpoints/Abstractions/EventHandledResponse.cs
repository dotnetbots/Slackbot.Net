namespace Slackbot.Net.Endpoints.Abstractions;

public class EventHandledResponse(string response)
{
    public string Response { get; } = response;
}
