using System;
using System.Threading.Tasks;
using Slackbot.Net.Endpoints.Abstractions;
using Slackbot.Net.Endpoints.Models.Events;

namespace CronBackgroundServices.Extensions.Samples.HelloWorld
{
    internal class HelloWorldHandler : IHandleEvent
    {
        public Task<EventHandledResponse> Handle(EventMetaData eventMetadata, SlackEvent slackEvent)
        {
            var message = slackEvent as AppMentionEvent;
            Console.WriteLine($"Hello world, {message.User}\n");
            return Task.FromResult(new EventHandledResponse("Responded"));
        }

        public bool ShouldHandle(SlackEvent slackEvent) => (slackEvent is AppMentionEvent appMention) && appMention.Text.Contains("hw");

        (string HandlerTrigger, string Description) IHandleEvent.GetHelpDescription() => ("hw", "hw");
    }
}