using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CronBackgroundServices
{
    internal class CronBackgroundService : BackgroundService
    {
        protected readonly IRecurringAction Action;
        private readonly ILogger _logger;
        private readonly Timing _timing;

        public CronBackgroundService(IRecurringAction action, ILogger logger)
        {
            _timing = new Timing(action.GetTimeZoneId());
            Action = action;
            _logger = logger;
            Cron = action.Cron;
            _logger.LogTrace($"Using {Cron} and timezone '{_timing.TimeZoneInfo.Id}. The time in this timezone: {_timing.RelativeNow()}'");
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
                    var uText = _timing.Get10NextOccurrences(Cron);
                    var logText = $"Ten next occurrences :\n{uText.Aggregate((x,y) => x + "\n" + y)}";
                    _logger.LogTrace(logText);
                }

                if (now > next)
                {
                    try
                    {
                        await Action.Process(stoppingToken);
                    }
                    catch (Exception e)
                    {
                        _logger.LogError(e, e.Message);
                    }
                    
                    next = _timing.GetNextOccurenceInRelativeTime(Cron);
                    _logger.LogTrace($"Next at {next.Value.DateTime.ToLongDateString()} {next.Value.DateTime.ToLongTimeString()}");
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