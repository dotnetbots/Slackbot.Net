using System.Threading.Tasks;
using Slackbot.Net.Endpoints.Models.Events;

namespace Slackbot.Net.Endpoints.Abstractions
{
    public interface IShortcutHandler
    {
        Task Handle(EventMetaData eventMetadata, SlackEvent @event);
        bool ShouldHandle(SlackEvent @event);
    }
}