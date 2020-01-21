using System;
using System.Threading.Tasks;
using Slackbot.Net.Abstractions.Handlers;
using Slackbot.Net.Abstractions.Handlers.Models.Rtm.MessageReceived;
using Slackbot.Net.Abstractions.Publishers;
using Slackbot.Net.Dynamic;

namespace Slackbot.Net.Extensions.Samples.HelloWorld
{
    internal class DebuggingStuffHandler : IHandleMessages
    {
        private readonly ISlackClientService _clientService;
        private readonly IPublisherBuilder _publisherFactory;
        public bool ShouldShowInHelp { get; } = true;
        public Tuple<string, string> GetHelpDescription() => new Tuple<string, string>("sc", "sc");

        public DebuggingStuffHandler(ISlackClientService clientService, IPublisherBuilder publisherFactory)
        {
            _clientService = clientService;
            _publisherFactory = publisherFactory;
        }
        
        public async Task<HandleResponse> Handle(SlackMessage message)
        {
            var slackClient = await _clientService.CreateClient(message.Team.Id);
            await slackClient.ChatPostMessage(message.ChatHub.Id, "SlackClient PONG");

            var publisher = await _publisherFactory.Build(message.Team.Id);
            await publisher.Publish(new Notification {Recipient = message.ChatHub.Id, Msg = "Publisher Pong"});
            
            return new HandleResponse("Responded");
        }

        public bool ShouldHandle(SlackMessage message)
        {
            return message.MentionsBot && message.Text.Contains("sc");
        }
    }
}