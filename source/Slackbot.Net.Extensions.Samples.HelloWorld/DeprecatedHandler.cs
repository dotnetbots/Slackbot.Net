using System;
using System.Threading.Tasks;
using Slackbot.Net.Abstractions.Handlers;
using Slackbot.Net.Abstractions.Handlers.Models.Rtm.MessageReceived;
using Slackbot.Net.Abstractions.Publishers;

namespace Slackbot.Net.Extensions.Samples.HelloWorld
{
    internal class DeprecatedHandler : IHandleMessages
    {
        // private readonly IEnumerable<IPublisher> _publishers;
        private readonly IPublisher _publishers;
        public bool ShouldShowInHelp { get; } = true;
        public Tuple<string, string> GetHelpDescription() => new Tuple<string, string>("derp", "derp");

        public DeprecatedHandler(IPublisher publishers)
        {
            _publishers = publishers;
        }
        
        public async Task<HandleResponse> Handle(SlackMessage message)
        {
            // foreach (var publisher in _publishers)
            // {
                await _publishers.Publish(new Notification {Recipient = message.ChatHub.Id, Msg = "Publisher Pong"});

            // }
            return new HandleResponse("Responded");
        }

        public bool ShouldHandle(SlackMessage message)
        {
            return message.MentionsBot && message.Text.Contains("derp");
        }
    }
}