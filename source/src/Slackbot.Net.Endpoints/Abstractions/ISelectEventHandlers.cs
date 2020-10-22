using System.Collections.Generic;
using System.Threading.Tasks;
using Slackbot.Net.Endpoints.Models.Events;

namespace Slackbot.Net.Endpoints.Abstractions
{
    public interface ISelectEventHandlers
    {
        Task<IEnumerable<IHandleAppMentionEvent>> GetAppMentionEventHandlerFor(EventMetaData eventMetadata, SlackEvent slackEvent);
    }
}