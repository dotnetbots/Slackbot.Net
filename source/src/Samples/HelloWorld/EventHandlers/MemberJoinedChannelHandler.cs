using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Slackbot.Net.Endpoints.Abstractions;
using Slackbot.Net.Endpoints.Models.Events;

namespace HelloWorld.EventHandlers
{
    public class MemberJoinedChannelHandler : IHandleMemberJoinedChannelEvents
    {
        public Task<EventHandledResponse> Handle(EventMetaData eventMetadata, MemberJoinedChannelEvent slackEvent)
        {
            Console.WriteLine("Doing stuff from MemberJoinedChannelHandler: " + JsonConvert.SerializeObject(slackEvent));
            return Task.FromResult(new EventHandledResponse("Wrote stuff to log"));
        }
    }
}