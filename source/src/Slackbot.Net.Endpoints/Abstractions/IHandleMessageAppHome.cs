using Slackbot.Net.Endpoints.Models.Events;

namespace Slackbot.Net.Endpoints.Abstractions;

public interface IHandleMessageAppHome
{
    Task<EventHandledResponse> Handle(EventMetaData eventMetadata, MessageAppHomeEvent appHomeMessage);
}
