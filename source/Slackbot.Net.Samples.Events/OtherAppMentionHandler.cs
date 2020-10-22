using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Slackbot.Net.Endpoints.Abstractions;
using Slackbot.Net.Endpoints.Models.Events;

namespace CronBackgroundServices.Samples.Events
{
    public class OtherAppMentionHandler : IHandleEvent
    {
        public Task<EventHandledResponse> Handle(EventMetaData eventMetadata, SlackEvent slackEvent)
        {
            Console.WriteLine("Doing stuff from AppmentionHandler: " + JsonConvert.SerializeObject(slackEvent));
            return Task.FromResult(new EventHandledResponse("Wrote stuff to log"));
        }

        public bool ShouldHandle(SlackEvent slackEvent) => slackEvent is AppMentionEvent appMentionEvent && appMentionEvent.Text == "test";
        
        public (string HandlerTrigger, string Description)  GetHelpDescription() => (HandlerTrigger: "donotshowinhelp", Description:  "hidden functionality");
    }
}