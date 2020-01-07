using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Slackbot.Net.Abstractions.Handlers;
using Slackbot.Net.Configuration;
using Slackbot.Net.Utilities;

namespace Slackbot.Net
{
    internal class RecurringAction : BackgroundService
    {

        private readonly IRecurringAction _action;
        private readonly ILogger _logger;
        private readonly Timing _timing;

        public RecurringAction(IRecurringAction action, ILogger logger)
        {
            var cronOptions = new CronOptions { Cron = action.Cron };
            _timing = new Timing();
            _timing.SetTimeZone(cronOptions.TimeZoneId);
            _action = action;
            _logger = logger;
            Cron = action.Cron;
            _logger.LogDebug($"Using {Cron} and timezone '{_timing.TimeZoneInfo.Id}. The time in this timezone: {_timing.RelativeNow()}'");
        }

        private string Cron { get; }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            DateTimeOffset? next = null;

            do
            {
                var now = _timing.RelativeNow();

                if (next == null)
                {
                    next = _timing.GetNextOccurenceInRelativeTime(Cron);

                    var upcoming = Timing.GetNextOccurences(Cron);
                    var uText = upcoming.Select(u => $"{u.ToLongDateString()} {next.Value.DateTime.ToLongTimeString()}").Take(10);
                    _logger.LogInformation($"Next at {next.Value.DateTime.ToLongDateString()} {next.Value.DateTime.ToLongTimeString()}\n" +
                                           $"Upcoming:\n{uText.Aggregate((x,y) => x + "\n" + y)}");
                }

                if (now > next)
                {
                    await _action.Process();
                    next = _timing.GetNextOccurenceInRelativeTime(Cron);
                    _logger.LogInformation($"Next at {next.Value.DateTime.ToLongDateString()} {next.Value.DateTime.ToLongTimeString()}");
                }
                else
                {
                    // needed for graceful shutdown for some reason.
                    // 100ms chosen so it doesn't affect calculating the next
                    // cron occurence (lowest possible: every second)
                    await Task.Delay(100, stoppingToken);
                }

            } while (!stoppingToken.IsCancellationRequested);
        }

    }
}