namespace Slackbot.Net.Azure.Functions.Abstractions;

public class EventHandledResponse
{
    public string Response { get; }

    public EventHandledResponse(string response)
    {
        Response = response;
    }
}
