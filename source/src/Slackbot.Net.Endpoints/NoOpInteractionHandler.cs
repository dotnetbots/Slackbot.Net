using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Slackbot.Net.Abstractions.Handlers;
using Slackbot.Net.Endpoints.Interactive.ViewSubmissions;

namespace Slackbot.Net.Endpoints.Interactive
{
    internal class NoOpViewSubmissionHandler
    {
        private readonly ILogger<NoOpViewSubmissionHandler> _logger;

        public NoOpViewSubmissionHandler(ILogger<NoOpViewSubmissionHandler> logger)
        {
            _logger = logger;
        }
        
        public Task<HandleResponse> Handle(Interaction payload)
        {
            if (payload is UnknownInteractiveMessage unknown)
            {
                _logger.LogWarning($"No handler for event type `{payload.Type}`. Unsupported interactive message? Unknown raw:\n{unknown.RawJson}");
            }
            else
            {
                _logger.LogWarning($"No-op for event type `{payload.Type}`. Missing interactive handler registration?");
            }
            
            
            return Task.FromResult(new HandleResponse("No-op."));
        }
    }
}