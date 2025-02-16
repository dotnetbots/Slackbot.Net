using Microsoft.Extensions.Logging;
using Slackbot.Net.Endpoints.Abstractions;
using Slackbot.Net.Endpoints.Models.Interactive;

namespace Slackbot.Net.Endpoints;

internal class NoOpViewSubmissionHandler(ILogger<NoOpViewSubmissionHandler> logger)
{
    public Task<EventHandledResponse> Handle(Interaction payload)
    {
        if (payload is UnknownInteractiveMessage unknown)
        {
            logger.LogWarning(
                $"No handler for event type `{payload.Type}`. Unsupported interactive message? Unknown raw:\n{unknown.RawJson}");
        }
        else
        {
            logger.LogWarning($"No-op for event type `{payload.Type}`. Missing interactive handler registration?");
        }


        return Task.FromResult(new EventHandledResponse("No-op."));
    }
}
