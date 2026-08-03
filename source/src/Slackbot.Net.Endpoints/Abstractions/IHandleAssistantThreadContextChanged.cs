using Slackbot.Net.Endpoints.Models.Events;

namespace Slackbot.Net.Endpoints.Abstractions;

public interface IHandleAssistantThreadContextChanged
{
    Task<EventHandledResponse> Handle(EventMetaData eventMetadata, AssistantThreadContextChangedEvent slackEvent);
}
