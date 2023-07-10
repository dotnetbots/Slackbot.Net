using Slackbot.Net.Azure.Functions.Models.Events;

namespace Slackbot.Net.Azure.Functions.Abstractions;

public interface ISelectAppMentionEventHandlers
{
    Task<IEnumerable<IHandleAppMentions>> GetAppMentionEventHandlerFor(EventMetaData eventMetadata, AppMentionEvent slackEvent);
}
