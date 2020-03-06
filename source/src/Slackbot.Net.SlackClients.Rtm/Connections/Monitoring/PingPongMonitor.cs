using System;
using System.Threading.Tasks;

namespace Slackbot.Net.SlackClients.Rtm.Connections.Monitoring
{
    internal class PingPongMonitor : IPingPongMonitor
    {
        private readonly ITimer _timer;
        private readonly IDateTimeKeeper _dateTimeKeeper;

        private TimeSpan _pongTimeout;
        private Func<Task> _pingMethod;
        private Func<Task> _reconnectMethod;
        public bool IsReconnecting;
        private readonly object _reconnectLock = new object();

        public PingPongMonitor(ITimer timer, IDateTimeKeeper dateTimeKeeper)
        {
            _timer = timer;
            _dateTimeKeeper = dateTimeKeeper;
        }

        public Task StartMonitor(Func<Task> pingMethod, Func<Task> reconnectMethod, TimeSpan pongTimeout)
        {
            if (_dateTimeKeeper.HasDateTime())
            {
                throw new MonitorAlreadyStartedException();
            }

            _pingMethod = pingMethod;
            _reconnectMethod = reconnectMethod;
            _pongTimeout = pongTimeout;

            _timer.RunEvery(TimerTick, TimeSpan.FromSeconds(20));
            return Task.CompletedTask;
        }

        private void TimerTick()
        {
            if (NeedsToReconnect() && !IsReconnecting)
            {
                lock (_reconnectLock)
                {
                    IsReconnecting = true;
                    Console.WriteLine($"1. Is reconnecting {IsReconnecting}");
                    _reconnectMethod().GetAwaiter().GetResult();
                    IsReconnecting = false;
                    Console.WriteLine($"2. Is reconnecting {IsReconnecting}");
                }
            }

            _pingMethod();
        }

        private bool NeedsToReconnect()
        {
            var shouldReconnect = _dateTimeKeeper.HasDateTime() && _dateTimeKeeper.TimeSinceDateTime() > _pongTimeout;
            if(shouldReconnect)
                Console.WriteLine($"Time since last pong: {_dateTimeKeeper.TimeSinceDateTime()}");
            
            return shouldReconnect;
        }

        public void Pong()
        {
            _dateTimeKeeper.SetDateTimeToNow();
        }

        public void StopMonitor()
        {
            _timer.Dispose();
        }
    }
}