using Slackbot.Net.Endpoints.Models.Events;

namespace Slackbot.Net.Endpoints.Abstractions;

public interface IHandleAssistantThreadStarted
{
    Task<EventHandledResponse> Handle(EventMetaData eventMetadata, AssistantThreadStartedEvent slackEvent);
}
