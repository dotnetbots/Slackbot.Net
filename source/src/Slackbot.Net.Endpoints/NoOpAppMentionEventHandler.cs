using Microsoft.Extensions.Logging;
using Slackbot.Net.Endpoints.Abstractions;
using Slackbot.Net.Endpoints.Models.Events;

namespace Slackbot.Net.Endpoints;

public class NoOpAppMentionEventHandler(ILogger<NoOpAppMentionEventHandler> logger) : IHandleAppMentions
{
    public Task<EventHandledResponse> Handle(EventMetaData eventMetadata, AppMentionEvent slackEvent)
    {
        logger.LogWarning($"No-op for app mention with text `{slackEvent?.Text}`.");
        return Task.FromResult(new EventHandledResponse("no-op handled it"));
    }

    public bool ShouldHandle(AppMentionEvent slackEvent)
    {
        return true;
    }

    public (string, string) GetHelpDescription()
    {
        return ("nada", "Fallback when no handlers are matched for any event you subscribe to");
    }
}
