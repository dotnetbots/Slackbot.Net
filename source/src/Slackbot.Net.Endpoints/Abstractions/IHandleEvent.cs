using System.Threading.Tasks;
using Slackbot.Net.Endpoints.Models;

namespace Slackbot.Net.Endpoints.Abstractions
{
    public interface IHandleEvent
    {
        Task<EventHandledResponse> Handle(EventMetaData eventMetadata, SlackEvent slackEvent);
        bool ShouldHandle(SlackEvent slackEvent);
        (string HandlerTrigger, string Description) GetHelpDescription();
    }
}