using System.Threading.Tasks;
using Slackbot.Net.Endpoints.Models;

namespace Slackbot.Net.Endpoints.Abstractions
{
    public interface IShortcutHandlers
    {
        Task Handle(EventMetaData eventMetadata, SlackEvent @event);
        bool ShouldHandle(SlackEvent @event);
    }
}