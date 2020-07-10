using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Slackbot.Net.Endpoints.Abstractions;
using Slackbot.Net.Endpoints.Models;

namespace Slackbot.Net.Samples.Events
{
    public class MemberJoinedChannelHandler : IHandleEvent
    {
        public Task<EventHandledResponse> Handle(EventMetaData eventMetadata, SlackEvent slackEvent)
        {
            Console.WriteLine("Doing stuff from MemberJoinedChannelHandler: " + JsonConvert.SerializeObject(slackEvent));
            return Task.FromResult(new EventHandledResponse("Wrote stuff to log"));
        }

        public bool ShouldHandle(SlackEvent slackEvent) => slackEvent is MemberJoinedChannelEvent;

        public (string, string) GetHelpDescription() => ("donotshowinhelp", "is not triggered by a user - dont care text");
        
    }
}