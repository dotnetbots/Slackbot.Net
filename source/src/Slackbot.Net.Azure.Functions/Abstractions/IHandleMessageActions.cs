using Slackbot.Net.Azure.Functions.Models.Interactive.MessageActions;

namespace Slackbot.Net.Azure.Functions.Abstractions;

public interface IHandleMessageActions
{
    Task<EventHandledResponse> Handle(MessageActionInteraction blockActionEvent);
}
