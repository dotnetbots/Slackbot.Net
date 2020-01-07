using System;
using System.Threading.Tasks;
using Slackbot.Net.Abstractions.Handlers;
using Slackbot.Net.Abstractions.Handlers.Models.Rtm.MessageReceived;

namespace Slackbot.Net.Handlers
{
    public class NoOpHandler : IHandleMessages
    {
        public bool ShouldShowInHelp
        {
            get;
        }

        public Tuple<string, string> GetHelpDescription() => new Tuple<string, string>("nada", "Gjør ingenting");

        public Task<HandleResponse> Handle(SlackMessage message)
        {
            return Task.FromResult(new HandleResponse("OK"));
        }

        public bool ShouldHandle(SlackMessage message)
        {
            return !message.MentionsBot;
        }
    }
}