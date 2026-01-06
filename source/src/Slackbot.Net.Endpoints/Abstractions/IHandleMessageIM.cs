using Slackbot.Net.Endpoints.Models.Events;

namespace Slackbot.Net.Endpoints.Abstractions;

public interface IHandleMessageIM
{
    Task<EventHandledResponse> Handle(EventMetaData eventMetadata, MessageIMEvent appHomeMessage);
}
