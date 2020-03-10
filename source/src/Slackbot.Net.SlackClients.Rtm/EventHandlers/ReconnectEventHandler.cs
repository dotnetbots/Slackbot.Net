using System.Threading.Tasks;

namespace Slackbot.Net.SlackClients.Rtm.EventHandlers
{
    public delegate Task ReconnectEventHandler(string teamId, string teamName);
}