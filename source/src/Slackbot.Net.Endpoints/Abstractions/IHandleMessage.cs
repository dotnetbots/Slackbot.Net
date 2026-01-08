using Slackbot.Net.Endpoints.Models.Events;

namespace Slackbot.Net.Endpoints.Abstractions;

public interface IHandleMessage
{
    Task<EventHandledResponse> Handle(EventMetaData eventMetadata, MessageEvent appHomeMessage);
}
