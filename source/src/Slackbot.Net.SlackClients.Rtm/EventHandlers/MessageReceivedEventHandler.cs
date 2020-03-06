using System.Threading.Tasks;
using Slackbot.Net.SlackClients.Rtm.Models;

namespace Slackbot.Net.SlackClients.Rtm.EventHandlers
{
    public delegate Task MessageReceivedEventHandler(Message message);

    public delegate Task ReconnectFailureEventHandler(string slackError, string teamId, string teamName);
}