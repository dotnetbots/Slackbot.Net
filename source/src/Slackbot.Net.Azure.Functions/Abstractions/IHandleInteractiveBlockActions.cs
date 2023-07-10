using Slackbot.Net.Azure.Functions.Models.Interactive.BlockActions;

namespace Slackbot.Net.Azure.Functions.Abstractions;

public interface IHandleInteractiveBlockActions
{
    Task<EventHandledResponse> Handle(BlockActionInteraction blockActionEvent);
}
