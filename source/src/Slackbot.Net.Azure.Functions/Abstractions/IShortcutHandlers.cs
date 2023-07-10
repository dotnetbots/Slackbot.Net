using Slackbot.Net.Azure.Functions.Models.Events;

namespace Slackbot.Net.Azure.Functions.Abstractions;

public interface IShortcutAppMentions
{
    Task Handle(EventMetaData eventMetadata, AppMentionEvent @event);
    bool ShouldShortcut(AppMentionEvent @event);
}
