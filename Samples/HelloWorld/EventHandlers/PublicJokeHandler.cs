using Newtonsoft.Json;
using Slackbot.Net.Endpoints.Abstractions;
using Slackbot.Net.Endpoints.Models.Events;

namespace HelloWorld.EventHandlers;

public class PublicJokeHandler : IHandleAppMentions
{
    public Task<EventHandledResponse> Handle(EventMetaData eventMetadata, AppMentionEvent slackEvent)
    {
        Console.WriteLine("Doing stuff from AppmentionHandler: " + JsonConvert.SerializeObject(slackEvent));
        return Task.FromResult(new EventHandledResponse("Wrote stuff to log"));
    }

    public bool ShouldHandle(AppMentionEvent slackEvent) => slackEvent.Text == "test";

    public (string HandlerTrigger, string Description) GetHelpDescription() => ("telljoke", "bot tells a joke");
}
