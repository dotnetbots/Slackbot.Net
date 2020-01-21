using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Slackbot.Net.Abstractions.Handlers;
using Slackbot.Net.Abstractions.Handlers.Models.Rtm.MessageReceived;
using Slackbot.Net.Abstractions.Publishers;

namespace Slackbot.Net.Extensions.Samples.HelloWorld
{
    internal class HelloWorldHandler : IHandleMessages
    {
        private readonly IEnumerable<IPublisherBuilder> _publishers;
        public bool ShouldShowInHelp { get; } = true;
        public Tuple<string, string> GetHelpDescription() => new Tuple<string, string>("hw", "hw");

        public HelloWorldHandler(IEnumerable<IPublisherBuilder> publishers)
        {
            _publishers = publishers;
        }
        
        public async Task<HandleResponse> Handle(SlackMessage message)
        {
            foreach (var p in _publishers)
            {
                var publisher = await p.Build(message.Team.Id);

                await publisher.Publish(new Notification
                {
                    Msg = $"Hello world, {message.User.Name} ({message.User.FirstName} {message.User.LastName})\n" +
                          $"I am {message.Bot.Name}",
                    Recipient = message.ChatHub.Id
                });   
            }
            
            return new HandleResponse("Responded");
        }

        public bool ShouldHandle(SlackMessage message)
        {
            return message.MentionsBot && message.Text.Contains("hw");
        }
    }
}