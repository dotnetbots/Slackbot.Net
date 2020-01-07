using System;
using System.Threading.Tasks;

namespace Slackbot.Net.SlackClients.Rtm.EventHandlers
{
    public delegate Task PongEventHandler(DateTime timestamp);
}
