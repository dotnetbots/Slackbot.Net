using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Slackbot.Net.Abstractions.Handlers;
using Slackbot.Net.Abstractions.Handlers.Models.Rtm.MessageReceived;
using Slackbot.Net.Abstractions.Publishers;
using Slackbot.Net.SlackClients.Http;

namespace Slackbot.Net.Handlers
{
    public class HelpHandler
    {
        private readonly IEnumerable<IHandleMessages> _handlers;
        private readonly ISlackClient _slackClient;

        public HelpHandler(IEnumerable<IHandleMessages> handlers, ISlackClient slackClient)
        {
            _handlers = handlers;
            _slackClient = slackClient;
        }

        public async Task<HandleResponse> Handle(SlackMessage message)
        {
            var text = _handlers.Where(handler => handler.ShouldShowInHelp)
                .Select(handler => handler.GetHelpDescription())
                .Aggregate("*HALP:*", (current, helpDescription) => current + $"\n• `{helpDescription.Item1}` : _{helpDescription.Item2}_");

            var helpText = new Notification
            {
                Recipient = message.ChatHub.Id,
                Msg = text
            };

            await _slackClient.ChatPostMessage(message.ChatHub.Id, text);
            
            return new HandleResponse("OK");
        }

        public bool ShouldHandle(SlackMessage message)
        {
            return message.MentionsBot && message.Text.Contains("help");
        }
    }
}