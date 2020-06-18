using System.Threading.Tasks;
using Slackbot.Net.Endpoints.Abstractions;
using Slackbot.Net.Endpoints.Models;

namespace Slackbot.Net.Endpoints
{
    public class NoOpShortcuttingHandler : IShortcutHandlers 
    {
        public Task Handle(EventMetaData eventMetadata, SlackEvent @event)
        {
            return Task.CompletedTask;
        }

        public bool ShouldHandle(SlackEvent @event)
        {
            return false;
        }
    }
}