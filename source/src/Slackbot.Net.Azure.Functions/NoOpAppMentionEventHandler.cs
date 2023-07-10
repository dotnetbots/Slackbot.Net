using Microsoft.Extensions.Logging;
using Slackbot.Net.Azure.Functions.Abstractions;
using Slackbot.Net.Azure.Functions.Models.Events;

namespace Slackbot.Net.Azure.Functions;

public class NoOpAppMentionEventHandler : IHandleAppMentions
{
    private readonly ILogger<NoOpAppMentionEventHandler> _logger;

    public NoOpAppMentionEventHandler(ILogger<NoOpAppMentionEventHandler> logger)
    {
        _logger = logger;
    }
    public Task<EventHandledResponse> Handle(EventMetaData eventMetadata, AppMentionEvent slackEvent)
    {
        _logger.LogWarning($"No-op for app mention with text `{slackEvent?.Text}`.");
        return Task.FromResult(new EventHandledResponse("no-op handled it"));
    }

    public bool ShouldHandle(AppMentionEvent slackEvent)
    {
        return true;
    }

    public (string, string) GetHelpDescription() => ("nada", "Fallback when no handlers are matched for any event you subscribe to");
}
