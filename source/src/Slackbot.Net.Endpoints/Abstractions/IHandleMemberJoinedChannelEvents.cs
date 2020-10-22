using System.Threading.Tasks;
using Slackbot.Net.Endpoints.Models.Events;

namespace Slackbot.Net.Endpoints.Abstractions
{
    public interface IHandleMemberJoinedChannelEvents
    {
        Task<EventHandledResponse> Handle(EventMetaData eventMetadata, MemberJoinedChannelEvent memberjoined);
    }
}