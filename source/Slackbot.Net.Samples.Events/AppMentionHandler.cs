using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Slackbot.Net.Endpoints.Abstractions;
using Slackbot.Net.Endpoints.Models;

namespace Slackbot.Net.Samples.Events
{
    public class AppMentionHandler : IHandleEvent
    {
        public Task Handle(EventMetaData eventMetadata, SlackEvent slackEvent)
        {
            Console.WriteLine("Doing stuff from AppmentionHandler: " + JsonConvert.SerializeObject(slackEvent));
            return Task.CompletedTask;
        }

        public bool ShouldHandle(SlackEvent slackEvent) => slackEvent is AppMentionEvent appMentionEvent && appMentionEvent.Text == "test";

        public Tuple<string, string> GetHelpDescription() => new Tuple<string, string>("does stuff when bot is app is mentioned", "does all");
    }
}