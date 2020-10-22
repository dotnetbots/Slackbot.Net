using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Slackbot.Net.Endpoints.Abstractions;
using Slackbot.Net.Endpoints.Models.Events;

namespace Slackbot.Net.Endpoints
{
    public class NoOpEventHandler : IHandleAppMentionEvent
    {
        private readonly ILogger<NoOpEventHandler> _logger;

        public NoOpEventHandler(ILogger<NoOpEventHandler> logger)
        {
            _logger = logger;
        }
        public Task<EventHandledResponse> Handle(EventMetaData eventMetadata, SlackEvent slackEvent)
        {
            if (slackEvent is UnknownSlackEvent unknown)
            {
                _logger.LogWarning($"No handler for event type `{slackEvent.Type}`. Unknown raw:\n{unknown.RawJson}");
            }
            else
            {
                _logger.LogWarning($"No-op for event type `{slackEvent.Type}`.");
            }
            
            return Task.FromResult(new EventHandledResponse("no-op handled it"));
        }

        public bool ShouldHandle(SlackEvent slackEvent)
        {
            return true;
        }

        public (string, string) GetHelpDescription() => ("nada", "Fallback when no handlers are matched for any event you subscribe to");
    }
}