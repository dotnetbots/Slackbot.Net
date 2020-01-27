using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Slackbot.Net.Abstractions.Publishers;

namespace Slackbot.Net.Extensions.Publishers.Logger
{
    public class LoggerPublisher : IPublisher
    {
        private readonly ILogger<LoggerPublisher> _logger;

        public LoggerPublisher(ILogger<LoggerPublisher> logger)
        {
            _logger = logger;
        }

        public Task Publish(Notification notification)
        {
            _logger.LogInformation($"[{DateTime.UtcNow.ToLongTimeString()}] {notification.Msg}");
            return Task.CompletedTask;
        }
    }
}