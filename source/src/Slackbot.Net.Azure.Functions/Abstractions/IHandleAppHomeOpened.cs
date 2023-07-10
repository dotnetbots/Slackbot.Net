using Slackbot.Net.Azure.Functions.Models.Events;

namespace Slackbot.Net.Azure.Functions.Abstractions;

public interface IHandleAppHomeOpened
{
    Task<EventHandledResponse> Handle(EventMetaData eventMetadata, AppHomeOpenedEvent payload);
}
