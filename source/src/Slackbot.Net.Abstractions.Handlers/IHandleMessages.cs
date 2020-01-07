using System;
using System.Threading.Tasks;
using Slackbot.Net.Abstractions.Handlers.Models.Rtm.MessageReceived;

namespace Slackbot.Net.Abstractions.Handlers
{
    public interface IHandleMessages
    {
        bool ShouldShowInHelp { get;}
        Tuple<string,string> GetHelpDescription();
        Task<HandleResponse> Handle(SlackMessage message);
        bool ShouldHandle(SlackMessage message);
    }
}