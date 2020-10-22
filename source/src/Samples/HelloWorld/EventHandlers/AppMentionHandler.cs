using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Slackbot.Net.Endpoints.Abstractions;
using Slackbot.Net.Endpoints.Models.Events;

namespace CronBackgroundServices.Samples.Events
{
    public class AppMentionHandler : IHandleAppMentionEvent
    {
        public Task<EventHandledResponse> Handle(EventMetaData eventMetadata, AppMentionEvent slackEvent)
        {
            Console.WriteLine("Doing stuff from AppmentionHandler: " + JsonConvert.SerializeObject(slackEvent));
            return Task.FromResult(new EventHandledResponse("Wrote stuff to log"));
        }

        public bool ShouldHandle(AppMentionEvent slackEvent) => slackEvent.Text == "test";

        public (string, string) GetHelpDescription() => ("does stuff when bot is app is mentioned", "does all");
    }
}