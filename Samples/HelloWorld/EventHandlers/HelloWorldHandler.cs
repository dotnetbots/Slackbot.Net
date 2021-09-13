using Slackbot.Net.Endpoints.Abstractions;
using Slackbot.Net.Endpoints.Models.Events;

namespace HelloWorld.EventHandlers;

internal class HelloWorldHandler : IHandleAppMentions
{
    public Task<EventHandledResponse> Handle(EventMetaData eventMetadata, AppMentionEvent message)
    {
        Console.WriteLine($"Hello world, {message.User}\n");
        return Task.FromResult(new EventHandledResponse("Responded"));
    }

    public bool ShouldHandle(AppMentionEvent appMention) => appMention.Text.Contains("hw");

    public (string HandlerTrigger, string Description) GetHelpDescription() => ("hw", "Returns a hello world message");
}
