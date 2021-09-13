using Newtonsoft.Json;
using Slackbot.Net.Endpoints.Abstractions;
using Slackbot.Net.Endpoints.Models.Events;

namespace HelloWorld.EventHandlers;

public class HiddenTestHandler : IHandleAppMentions
{
    public Task<EventHandledResponse> Handle(EventMetaData eventMetadata, AppMentionEvent slackEvent)
    {
        Console.WriteLine("Doing stuff from OtherAppMentionHandler: " + JsonConvert.SerializeObject(slackEvent));
        return Task.FromResult(new EventHandledResponse("Wrote stuff to log"));
    }

    public bool ShouldHandle(AppMentionEvent appMentionEvent) => appMentionEvent.Text == "test";
}
