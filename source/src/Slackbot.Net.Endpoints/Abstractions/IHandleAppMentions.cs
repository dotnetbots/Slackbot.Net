using Slackbot.Net.Endpoints.Models.Events;

namespace Slackbot.Net.Endpoints.Abstractions;

public interface IHandleAppMentions
{
    Task<EventHandledResponse> Handle(EventMetaData eventMetadata, AppMentionEvent slackEvent);

    bool ShouldHandle(AppMentionEvent slackEvent)
    {
        return true;
    }

    (string HandlerTrigger, string Description) GetHelpDescription()
    {
        return ("", "");
    }
}
