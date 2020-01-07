using System;

namespace Slackbot.Net.SlackClients.Rtm.Connections.Monitoring
{
    internal interface IDateTimeKeeper
    {
        void SetDateTimeToNow();
        bool HasDateTime();
        TimeSpan TimeSinceDateTime();
    }
}