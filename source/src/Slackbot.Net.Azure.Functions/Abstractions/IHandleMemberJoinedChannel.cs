using Slackbot.Net.Azure.Functions.Models.Events;

namespace Slackbot.Net.Azure.Functions.Abstractions;

public interface IHandleMemberJoinedChannel
{
    Task<EventHandledResponse> Handle(EventMetaData eventMetadata, MemberJoinedChannelEvent memberjoined);
}
