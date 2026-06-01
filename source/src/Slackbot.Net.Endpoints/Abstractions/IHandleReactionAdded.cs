using Slackbot.Net.Endpoints.Models.Events;

namespace Slackbot.Net.Endpoints.Abstractions;

public interface IHandleReactionAdded
{
    Task<EventHandledResponse> Handle(EventMetaData eventMetadata, ReactionAddedEvent slackEvent);

    bool ShouldHandle(ReactionAddedEvent slackEvent)
    {
        return true;
    }
}
