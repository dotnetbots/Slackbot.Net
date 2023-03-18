using Slackbot.Net.Endpoints.Models.Interactive.MessageActions;

namespace Slackbot.Net.Endpoints.Abstractions;

public interface IHandleMessageActions
{
    Task<EventHandledResponse> Handle(MessageActionInteraction blockActionEvent);
}