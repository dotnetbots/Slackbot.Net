using Slackbot.Net.Endpoints.Models.Events;

namespace Slackbot.Net.Endpoints.Abstractions;

public interface IHandleTeamJoin
{
    Task<EventHandledResponse> Handle(EventMetaData eventMetadata, TeamJoinEvent teamJoined);
}
