using Slackbot.Net.Endpoints.Models.Events;

namespace Slackbot.Net.Endpoints.Abstractions;

public interface IHandleEmojiChanged
{
    Task<EventHandledResponse> Handle(EventMetaData eventMetadata, EmojiChangedEvent emojiChanged);
}
