using System.Threading.Tasks;
using Slackbot.Net.Endpoints.Models.Events;

namespace Slackbot.Net.Endpoints.Abstractions
{
    public interface IHandleAppMentionEvent
    {
        Task<EventHandledResponse> Handle(EventMetaData eventMetadata, SlackEvent slackEvent);
        bool ShouldHandle(SlackEvent slackEvent);
        (string HandlerTrigger, string Description) GetHelpDescription();
    }
}