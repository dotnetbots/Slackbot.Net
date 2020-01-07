using System;

namespace Slackbot.Net.SlackClients.Rtm.Connections.Monitoring
{
    internal interface ITimer : IDisposable
    {
        void RunEvery(Action action, TimeSpan tick);
    }
}