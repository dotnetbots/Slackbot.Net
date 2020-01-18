using System;
using System.Threading.Tasks;
using Slackbot.Net.Abstractions.Handlers;
using Slackbot.Net.Abstractions.Handlers.Models.Rtm.MessageReceived;
using Slackbot.Net.Abstractions.Publishers;

namespace Slackbot.Net.Extensions.Samples.HelloWorld
{
    internal class HelloWorldHandler : IHandleMessages
    {
        private readonly IPublisher _publisher;
        public bool ShouldShowInHelp { get; }
        public Tuple<string, string> GetHelpDescription() => new Tuple<string, string>("hw", "hw");

        public HelloWorldHandler(IPublisher publisher)
        {
            _publisher = publisher;
        }
        
        public async Task<HandleResponse> Handle(SlackMessage message)
        {
            await _publisher.Publish(new Notification
            {
                Msg = $"Hello world, {message.User.Name} ({message.User.FirstName} {message.User.LastName})\n" +
                      $"I am {message.Bot.Name}"
            });
            return new HandleResponse("Responded");
        }

        public bool ShouldHandle(SlackMessage message)
        {
            return message.MentionsBot && message.Text.Contains("hw");
        }
    }
}