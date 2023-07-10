using Slackbot.Net.Azure.Functions.Models.Events;

namespace Slackbot.Net.Azure.Functions.Abstractions;

public interface IHandleAppMentions
{
    Task<EventHandledResponse> Handle(EventMetaData eventMetadata, AppMentionEvent slackEvent);
    bool ShouldHandle(AppMentionEvent slackEvent) => true;
    (string HandlerTrigger, string Description) GetHelpDescription() => ("", "");
}
